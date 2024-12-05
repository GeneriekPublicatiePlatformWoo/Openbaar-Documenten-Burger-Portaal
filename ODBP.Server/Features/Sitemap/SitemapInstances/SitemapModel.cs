using System.Xml.Serialization;

namespace ODBP.Features.Sitemap.SitemapInstances;

[XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public class SitemapModel
{
    [XmlElement("url")]
    public required List<Publicatie> Urls { get; init; }
}

public class Publicatie
{
    [XmlElement("loc")]
    public required string Loc { get; init; }

    /// <summary>
    /// Moet voldoen aan https://www.w3.org/TR/NOTE-datetime
    /// </summary>
    [XmlElement("lastmod")]
    public required string Lastmod { get; init; }

    [XmlElement(ElementName = "Document", Namespace = DiwooConstants.Namespace)]
    public required Document Document { get; init; }
}

public class Document
{
    [XmlElement("DiWoo")]
    public required DiWoo DiWoo { get; init; }
}

public class DiWoo
{
    [XmlElement("creatiedatum")]
    public required string Creatiedatum { get; set; }

    [XmlArray("identifiers")]
    [XmlArrayItem("identifier")]
    public required string[]? Identifiers { get; set; }

    [XmlElement("publisher")]
    public required ResourceWithValue? Publisher { get; init; }

    [XmlElement("verantwoordelijke")]
    public required ResourceWithValue? Verantwoordelijke { get; init; }

    [XmlElement("opsteller")]
    public required ResourceWithValue? Opsteller { get; init; }

    [XmlArray("omschrijvingen")]
    [XmlArrayItem("omschrijving")]
    public required string[]? Omschrijvingen { get; set; }

    [XmlElement("titelcollectie")]
    public required Titelcollectie Titelcollectie { get; init; }

    [XmlElement("classificatiecollectie")]
    public required Classificatiecollectie Classificatiecollectie { get; set; }

    [XmlArray("documenthandelingen")]
    [XmlArrayItem("documenthandeling")]
    public required Documenthandeling[] Documenthandelingen { get; set; }
}

public class Titelcollectie
{
    [XmlElement("officieleTitel")]
    public required string OfficieleTitel { get; init; }

    [XmlArray("verkorteTitels")]
    [XmlArrayItem("verkorteTitel")]
    public required string[]? VerkorteTitels { get; init; }
}

public class Classificatiecollectie
{
    [XmlArray("informatiecategorieen")]
    [XmlArrayItem("informatiecategorie")]
    public required ResourceWithValue[] Informatiecategorieen { get; init; }
}

public class Documenthandeling
{
    [XmlElement("soortHandeling")]
    public required ResourceWithValue SoortHandeling { get; init; }

    [XmlElement("atTime")]
    public required string AtTime { get; init; }
}

public class ResourceWithValue
{
    [XmlAttribute("resource")]
    public required string Resource { get; init; }

    [XmlText]
    public required string Value { get; init; }
}
