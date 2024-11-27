using System.Xml.Serialization;

namespace ODBP.Features.Sitemap.SitemapInstances
{
    public class DiwooXmlResult(SitemapModel model) : XmlResult<SitemapModel>(model, s_namespaces)
    {
        private static readonly XmlSerializerNamespaces s_namespaces = GetNamespaces();

        private static XmlSerializerNamespaces GetNamespaces()
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("diwoo", DiwooConstants.Namespace);
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            return namespaces;
        }
    }
}
