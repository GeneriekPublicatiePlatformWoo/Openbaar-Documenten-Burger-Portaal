using Microsoft.AspNetCore.Mvc;

namespace ODBP.Features.Sitemap.SitemapIndex
{
    [ApiController]
    public class SitemapIndexController(BaseUri baseUri)
    {
        [HttpGet(ApiRoutes.SitemapIndex)]
        public IActionResult Get()
        {
            var sitemapUri = new Uri(baseUri, ApiRoutes.Sitemap);
            var data = new SitemapIndexModel { Sitemaps = [new() { Loc = sitemapUri.ToString() }] };
            return new XmlResult<SitemapIndexModel>(data);
        }
    }
}
