using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ODBP.Apis.Odrc;

namespace ODBP.Features.Documenten
{
    [ApiController]
    public class DownloadDocumentController(IOdrcClientFactory clientFactory, ILogger<DownloadDocumentController> logger) : ControllerBase
    {
        const string Gepubliceerd = "gepubliceerd";

        [HttpGet("/api/{version}/documenten/{id}/download")]
        public async Task<IActionResult> Get(string version, string id, CancellationToken token)
        {
            using var client = clientFactory.Create("Publicatiestatus checken van document");
            using var documentResponse = await client.GetAsync($"/api/{version}/documenten/{id}", HttpCompletionOption.ResponseContentRead, token);

            if (documentResponse.StatusCode == HttpStatusCode.NotFound)
            {
                // document niet gevonden
                logger.LogWarning("Document opgevraagd maar niet gevonden, id: {DocumentId}", id);
                return NotFound();
            }

            if (!documentResponse.IsSuccessStatusCode)
            {
                // er is iets mis in ODRC
                var errorBody = await documentResponse.Content.ReadAsStringAsync(token);
                logger.LogError("Fout bij ophalen document uit ODRC, status: {Status}, body: {body}", documentResponse.StatusCode, errorBody);
                return StatusCode(502);
            }

            var document = await documentResponse.Content.ReadFromJsonAsync(PublicatieContext.Default.DocumentModel, token);

            if(document?.Publicatiestatus != Gepubliceerd)
            {
                // document is nog niet / niet meer gepubliceerd
                logger.LogWarning("Document opgevraagd maar niet gepubliceerd, id: {DocumentId}", id);
                return NotFound();
            }

            var publicatie = await client.GetFromJsonAsync($"/api/{version}/publicaties/{document.Publicatie}", PublicatieContext.Default.PublicatieModel, token);

            if (publicatie?.Publicatiestatus != Gepubliceerd)
            {
                // bijbehorende publicatie is nog niet / niet meer gepubliceerd
                logger.LogWarning("Document opgevraagd maar publicatie niet gepubliceerd, documentId: {DocumentId}, publicatieId: {PublicatieId}", id, document.Publicatie);
                return NotFound();
            }

            return new DownloadResult(Request.Path, "Document downloaden");
        }
    }

    [JsonSerializable(typeof(PublicatieModel))]
    [JsonSerializable(typeof(DocumentModel))]
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    internal partial class PublicatieContext : JsonSerializerContext
    {
    }

    internal class PublicatieModel
    {
        public required string Publicatiestatus { get; init; }
    }

    internal class DocumentModel
    {
        public required string Publicatie { get; init; }
        public required string Publicatiestatus { get; init; }
    }
}
