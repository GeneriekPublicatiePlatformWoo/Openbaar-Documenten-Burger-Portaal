﻿using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ODBP.Apis.Odrc;

namespace ODBP.Features.Sitemap.SitemapInstances
{
    [ApiController]
    public class SitemapController(IOdrcClientFactory odrcClientFactory, BaseUri baseUri)
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
            using var odrcClient = odrcClientFactory.Create(handeling: "sitemap opbouwen");

            // de documenten bevatten alleen de uuid van de publicatie die erbij hoort
            // de publicaties bevatten alleen de uuid van de organisatie / informatiecategorieen die erbij horen
            // voor de sitemap hebben we de naam en identifier nodig van organisaties / informatiecategorieen
            // daarom halen we die los op, zodat we ze per document kunnen opzoeken
            var organisatiesTask = GetWaardelijstDictionary(odrcClient, OrganisatiesPath, token);
            var informatiecategorieenTask = GetWaardelijstDictionary(odrcClient, InformatieCategorieenPath, token);

            var gepubliceerdePublicaties = await GetGepubliceerdePublicatieDictionary(odrcClient, token);
            var organisaties = await organisatiesTask;
            var informatiecategorieen = await informatiecategorieenTask;

            var publicaties = new List<Publicatie>();

            // doorloop alle documenten
            await foreach (var item in GetAllPages(odrcClient, DocumentenQueryPath, token))
            {
                var document = item.Deserialize(SitemapPublicatieContext.Default.OdrcDocument);
                // als we de publicatie niet bij het document kunnen vinden, is de publicatie niet bekend of heeft deze niet de status gepubliceerd
                // dan negeren we het document
                if (document == null || !gepubliceerdePublicaties.TryGetValue(document.Publicatie, out var publicatie))
                {
                    continue;
                }
                publicaties.Add(new()
                {
                    Loc = new Uri(baseUri, $"{DocumentenRoot}/{document.Uuid}/download").ToString(),
                    // we nemen zowel gegevens van het document als van de publicatie over in de sitemap
                    // het lastmod veld is input voor de crawler om te bepalen of er iets opnieuw geindexeerd moet worden
                    // de laatste wijzigingsdatum van het document / de publicatie is dus leidend
                    Lastmod = MaxDateTimeOffset(document.LaatstGewijzigdDatum, publicatie.LaatstGewijzigdDatum).ToString("o"),
                    Document = new()
                    {
                        DiWoo = new()
                        {
                            // als we om de een of andere reden geen organisatie kunnen vinden obv van de publisher id, laten we deze leeg
                            // we voorzien niet dat dit gebeurt maar het is altijd beter om een document met minder metadata te tonen dan helemaal niet
                            Publisher = organisaties.TryGetValue(publicatie.Publisher, out var publisher)
                                ? publisher
                                : null,
                            Titelcollectie = new()
                            {
                                OfficieleTitel = document.OfficieleTitel,
                            },
                            Classificatiecollectie = new()
                            {
                                // zoek de informatiecategorieen op obv de ids die in de publicatie staan.
                                // als we er eentje niet kunnen vinden, negeren we deze
                                Informatiecategorieen =
                                    LookupValuesInDictionary(publicatie.InformatieCategorieen, informatiecategorieen)
                                    .ToArray()
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
                Urls = publicaties
            };

            return new DiwooXmlResult(model);
        }

        /// <summary>
        /// Haalt een waardelijst op zodat we de waardes op basis van de id kunnen opzoeken.
        /// </summary>
        private static async Task<IReadOnlyDictionary<string, ResourceWithValue>> GetWaardelijstDictionary(HttpClient client, string path, CancellationToken token)
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

        /// <summary>
        /// Haalt gepubliceerde publicaties zodat we die op basis van de id kunnen opzoeken.
        /// </summary>
        private static async Task<IReadOnlyDictionary<string, OdrcPublicatie>> GetGepubliceerdePublicatieDictionary(HttpClient client, CancellationToken token)
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

        /// <summary>
        /// Doorloopt alle pagina's van een API-response en retourneert de resultaten.
        /// </summary>
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

        /// <summary>
        /// Zoekt waardes op in een dictionary op basis van een lijst van sleutels.
        /// </summary>
        private static IEnumerable<T2> LookupValuesInDictionary<T1, T2>(IEnumerable<T1> values, IReadOnlyDictionary<T1, T2> dictionary)
        {
            foreach (var item in values)
            {
                if (dictionary.TryGetValue(item, out var result))
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Retourneert de maximale waarde van twee DateTimeOffsets.
        /// </summary>
        private static DateTimeOffset MaxDateTimeOffset(DateTimeOffset left, DateTimeOffset right) => left > right ? left : right;
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