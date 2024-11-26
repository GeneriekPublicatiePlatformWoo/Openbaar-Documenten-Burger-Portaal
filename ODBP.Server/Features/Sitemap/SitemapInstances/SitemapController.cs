using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ODBP.Apis.Odrc;

namespace ODBP.Features.Sitemap.SitemapInstances
{
    [ApiController]
    public class SitemapController(IOdrcClientFactory clientFactory, BaseUri baseUri)
    {
        const string ApiVersion = "v1";
        const string ApiRoot = $"/api/{ApiVersion}";
        const string OrganisatiesPath = $"{ApiRoot}/organisaties?isActief=alle";
        const string InformatieCategorieenPath = $"{ApiRoot}/informatiecategorieen";
        const string PublicatiesPath = $"{ApiRoot}/publicaties?publicatiestatus=gepubliceerd&sorteer=registratiedatum";
        const string DocumentenRoot = $"{ApiRoot}/documenten";
        const string DocumentenQueryPath = $"{DocumentenRoot}?publicatiestatus=gepubliceerd&sorteer=creatiedatum";

        [HttpGet(ApiRoutes.Sitemap)]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            using var client = clientFactory.Create("sitemap opbouwen");
            var organisatiesTask = GetWaardelijst(client, OrganisatiesPath, token);
            var informatiecategorieenTask = GetWaardelijst(client, InformatieCategorieenPath, token);

            var publicaties = await GetPublicaties(client, token);
            var organisaties = await organisatiesTask;
            var informatiecategorieen = await informatiecategorieenTask;

            var urls = new List<Publicatie>();

            await foreach (var item in GetAllPages(client, DocumentenQueryPath, token))
            {
                var document = item.Deserialize(SitemapPublicatieContext.Default.OdrcDocument);
                if (document == null ||
                    !publicaties.TryGetValue(document.Publicatie, out var publicatie) ||
                    !organisaties.TryGetValue(publicatie.Publisher, out var publisher))
                {
                    continue;
                }
                urls.Add(new()
                {
                    Loc = new Uri(baseUri, $"{DocumentenRoot}/{document.Uuid}/download").ToString(),
                    Lastmod = Max(document.LaatstGewijzigdDatum, publicatie.LaatstGewijzigdDatum).ToString("o"),
                    Document = new()
                    {
                        DiWoo = new()
                        {
                            Publisher = publisher,
                            Titelcollectie = new()
                            {
                                OfficieleTitel = document.OfficieleTitel,
                            },
                            Classificatiecollectie = new()
                            {
                                Informatiecategorieen = Lookup(publicatie.InformatieCategorieen, informatiecategorieen).ToArray()
                            },
                            Documenthandelingen = document.Documenthandelingen.Select(x => new Documenthandeling
                            {
                                AtTime = x.AtTime,
                                SoortHandeling = new() { Resource = x.SoortHandeling, Value = x.SoortHandeling }
                            }).ToArray()
                        }
                    }
                });
            }

            var model = new SitemapModel
            {
                Urls = urls
            };

            return new DiwooXmlResult(model);
        }

        private static IEnumerable<T2> Lookup<T1, T2>(IEnumerable<T1> values, IReadOnlyDictionary<T1, T2> dictionary)
        {
            foreach (var item in values)
            {
                if (dictionary.TryGetValue(item, out var result))
                {
                    yield return result;
                }
            }
        }

        private static DateTimeOffset Max(DateTimeOffset left, DateTimeOffset right) => left > right ? left : right;

        private static async Task<IReadOnlyDictionary<string, ResourceWithValue>> GetWaardelijst(HttpClient client, string path, CancellationToken token)
        {
            var result = new Dictionary<string, ResourceWithValue>();
            await foreach (var item in GetAllPages(client, path, token))
            {
                if (item.TryGetProperty("uuid", out var uuidProp)
                    && item.TryGetProperty("identifier", out var identifierProp)
                    && item.TryGetProperty("naam", out var naamprop)
                    && uuidProp.ValueKind == JsonValueKind.String
                    && identifierProp.ValueKind == JsonValueKind.String
                    && naamprop.ValueKind == JsonValueKind.String
                    )
                {
                    result[uuidProp.GetString()!] = new() { Resource = identifierProp.GetString()!, Value = naamprop.GetString()! };
                }
            }
            return result;
        }

        private static async Task<IReadOnlyDictionary<string, OdrcPublicatie>> GetPublicaties(HttpClient client, CancellationToken token)
        {
            var result = new Dictionary<string, OdrcPublicatie>();
            await foreach (var item in GetAllPages(client, PublicatiesPath, token))
            {
                var publicatie = item.Deserialize(SitemapPublicatieContext.Default.OdrcPublicatie);
                if (publicatie != null)
                {
                    result[publicatie.Uuid] = publicatie;
                }
            }
            return result;
        }

        private static async IAsyncEnumerable<JsonElement> GetAllPages(HttpClient client, string url, [EnumeratorCancellation] CancellationToken token)
        {
            string? next = null;
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token))
            {
                response.EnsureSuccessStatusCode();
                await using var stream = await response.Content.ReadAsStreamAsync(token);
                using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: token);
                if (doc.RootElement.TryGetProperty("next", out var nextProp) && nextProp.ValueKind == JsonValueKind.String)
                {
                    next = nextProp.GetString();
                }
                if (doc.RootElement.TryGetProperty("results", out var resultsProp) && resultsProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in resultsProp.EnumerateArray())
                    {
                        yield return item;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(next))
            {
                await foreach (var item in GetAllPages(client, next, token))
                {
                    yield return item;
                }
            }
        }
    }

    [JsonSerializable(typeof(OdrcDocument))]
    [JsonSerializable(typeof(OdrcPublicatie))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    internal partial class SitemapPublicatieContext : JsonSerializerContext
    {

    }

    public class OdrcDocument
    {
        public required string Uuid { get; set; }
        public required string Publicatie { get; set; }
        public required string OfficieleTitel { get; set; }
        public required DateTimeOffset LaatstGewijzigdDatum { get; set; }
        public required IReadOnlyList<OdrcDocumentHandeling> Documenthandelingen { get; set; }
    }

    public class OdrcPublicatie
    {
        public required string Uuid { get; set; }
        public required string Publisher { get; set; }
        public string? Verantwoordelijke { get; set; }
        public required DateTimeOffset LaatstGewijzigdDatum { get; set; }
        public required IReadOnlyList<string> InformatieCategorieen { get; set; }
    }

    public class OdrcDocumentHandeling
    {
        public required string SoortHandeling { get; set; }
        public required string AtTime { get; set; }
    }
}
