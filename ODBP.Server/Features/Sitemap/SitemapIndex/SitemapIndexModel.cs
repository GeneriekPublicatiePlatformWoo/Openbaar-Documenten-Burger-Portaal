using System.Globalization;
using System.Xml.Serialization;

namespace ODBP.Features.Sitemap.SitemapIndex;

[XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public class SitemapIndexModel
{
    [XmlElement("sitemap")]
    public required List<SitemapLink> Sitemaps { get; set; }
}

public class SitemapLink
{
    [XmlElement("loc")]
    public required string Loc { get; set; }

    /// <summary>
    /// Moet voldoen aan https://www.w3.org/TR/NOTE-datetime
    /// </summary>
    [XmlElement("lastmod")]
    public string LastMod { get; set; } = DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture);
}
