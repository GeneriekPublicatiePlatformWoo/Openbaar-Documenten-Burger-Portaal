using Microsoft.AspNetCore.Mvc;
using ODBP.Apis.Odrc;

namespace ODBP.Features.Documenten
{
    public class DownloadResult(string path, string reason) : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var token = context.HttpContext.RequestAborted;

            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            using var client = context.HttpContext.RequestServices.GetRequiredService<IOdrcClientFactory>().Create(reason);
            var timeoutInMinutes = int.TryParse(config["DOWNLOAD_TIMEOUT_MINUTES"], out var m)
                ? m
                : 10;
            client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);
            using var httpResponse = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead, token);

            response.StatusCode = (int)httpResponse.StatusCode;
            response.Headers.ContentLength = httpResponse.Content.Headers.ContentLength;
            response.Headers.ContentDisposition = httpResponse.Content.Headers.ContentDisposition?.ToString();
            response.Headers.ContentType = httpResponse.Content.Headers.ContentType?.ToString();

            await httpResponse.Content.CopyToAsync(response.Body, token);
        }
    }
}
