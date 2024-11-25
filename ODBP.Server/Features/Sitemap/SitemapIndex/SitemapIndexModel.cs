using System.Xml.Serialization;

namespace ODBP.Features.Sitemap.SitemapIndex;


[XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public class SitemapIndexModel
{
    [XmlElement("sitemap")]
    public required List<SitemapInstance> Sitemaps { get; set; }
}

public class SitemapInstance
{
    [XmlElement("loc")]
    public required string Loc { get; set; }
}
