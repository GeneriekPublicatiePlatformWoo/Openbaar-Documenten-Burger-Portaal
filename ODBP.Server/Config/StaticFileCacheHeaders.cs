using System.Text.RegularExpressions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Builder;

public static partial class StaticFileCacheHeaders
{
    private const int DurationInSeconds = 60 * 60 * 24 * 100;

    private static readonly Regex s_hashHashRegex = HashRegex();

    private static readonly CacheControlHeaderValue s_longCache = new()
    {
        Public = true,
        MaxAge = TimeSpan.FromSeconds(DurationInSeconds)
    };

    private static readonly CacheControlHeaderValue s_noCache = new()
    {
        NoCache = true,
    };

    private static readonly StaticFileOptions s_staticFileOptions = new()
    {
        OnPrepareResponse = SetCacheHeaders
    };

    public static IApplicationBuilder UseOdbpStaticFiles(this IApplicationBuilder app) => app.UseStaticFiles(s_staticFileOptions);

    public static IEndpointConventionBuilder MapFallbackToIndexHtml(this IEndpointRouteBuilder builder) => builder
        .MapFallbackToFile("index.html", s_staticFileOptions).AllowAnonymous();

    private static void SetCacheHeaders(this StaticFileResponseContext ctx)
    {
        var headers = ctx.Context.Response.GetTypedHeaders();

        headers.CacheControl = ctx.File.Name.HasHash()
            ? s_longCache
            : s_noCache;
    }

    private static bool HasHash(this string fileName) => s_hashHashRegex.IsMatch(fileName);
    
    [GeneratedRegex(@"^[\w]+-[a-zA-Z0-9]{8}\.[\w]+$")]
    private static partial Regex HashRegex();
}
