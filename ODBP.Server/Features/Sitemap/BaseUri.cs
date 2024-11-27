namespace ODBP.Features.Sitemap
{
    public record BaseUri(Uri Value)
    {
        public static implicit operator Uri(BaseUri baseUri) => baseUri.Value;
    }

    public static class BaseUriExtensions
    {
        public static void AddBaseUri(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(s =>
            {
                var accessor = s.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext!.Request;
                var builder = new UriBuilder();
                var scheme = request.Scheme == "http" ? "http" : "https";
                builder.Scheme = scheme;
                builder.Host = request.Host.Host;

                if (request.Host.Port.HasValue)
                {
                    builder.Port = request.Host.Port.Value;
                }

                return new BaseUri(builder.Uri);
            });
        }
    }
}
