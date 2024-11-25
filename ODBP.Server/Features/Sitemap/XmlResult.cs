using System.IO.Hashing;
using System.Net.Mime;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ODBP.Features.Sitemap
{
    public class XmlResult<T>(T model, XmlSerializerNamespaces? namespaces = null) : IActionResult
    {
        private static readonly XmlSerializer s_serializer = new(typeof(T));
        private static readonly XmlWriterSettings s_xmlWriterSettings = new() { Async = true, NamespaceHandling = NamespaceHandling.OmitDuplicates };
        private const int DefaultBufferSize = 4096;

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var token = context.HttpContext.RequestAborted;
            context.HttpContext.Response.Headers.ContentType = MediaTypeNames.Text.Xml;
            var fn = Path.GetTempFileName();
            await using var file = File.Create(fn, DefaultBufferSize, FileOptions.DeleteOnClose);
            await using var writer = XmlWriter.Create(file, s_xmlWriterSettings);
            s_serializer.Serialize(writer, model, namespaces);
            file.Seek(0, SeekOrigin.Begin);
            var hash = await Hash(file, token);
            context.HttpContext.Response.Headers.ETag = $"\"{WebEncoders.Base64UrlEncode(hash)}\"";
            file.Seek(0, SeekOrigin.Begin);
            context.HttpContext.Response.Headers.ContentLength = file.Length;
            await file.CopyToAsync(context.HttpContext.Response.Body, token);
        }

        private static async Task<byte[]> Hash(Stream stream, CancellationToken token)
        {
            var hash = new XxHash3();
            await hash.AppendAsync(stream, token);
            return hash.GetCurrentHash();
        }
    }
}
