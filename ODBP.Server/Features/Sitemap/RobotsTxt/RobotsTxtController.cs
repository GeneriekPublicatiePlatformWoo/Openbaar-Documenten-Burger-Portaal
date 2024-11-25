using Microsoft.AspNetCore.Mvc;

namespace ODBP.Features.Sitemap.RobotsTxt
{
    [ApiController]
    public class RobotsTxtController(BaseUri baseUri): ControllerBase
    {
        [HttpGet("/robots.txt")]
        public IActionResult Get()
        {
            var sitemapIndexUri = new Uri(baseUri, ApiRoutes.SitemapIndex);
            return Ok($"""
            User-agent: *
            Disallow:
            Sitemap: {sitemapIndexUri}
            """);
        }
    }
}
