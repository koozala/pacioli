using System.Xml;
using System.Xml.Linq;

namespace Pacioli.CLI.Test.Content
{
    /*
     * Reads all values of an XML input document.
     *
     * A value is the text of an element that has no child element, or the value of an
     * attribute. The extractor gives the path of each value. The path uses the local
     * names, without the namespace prefix, for example:
     *
     *   /CrossIndustryInvoice/ExchangedDocument/ID
     *   /CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format
     *
     * The path has no position number. Thus all positions of an invoice have the same
     * path, and you can exclude an element with one entry.
     */
    public static class XmlValueExtractor
    {
        public static IReadOnlyList<XmlValue> Extract(string xmlPath)
        {
            XDocument document = XDocument.Load(xmlPath, LoadOptions.SetLineInfo);
            var values = new List<XmlValue>();

            if (document.Root != null)
            {
                Collect(document.Root, string.Empty, values);
            }

            return values;
        }

        private static void Collect(XElement element, string parentPath, List<XmlValue> values)
        {
            string path = $"{parentPath}/{element.Name.LocalName}";

            foreach (XAttribute attribute in element.Attributes())
            {
                if (attribute.IsNamespaceDeclaration)
                {
                    continue;
                }

                Add(values, $"{path}@{attribute.Name.LocalName}", attribute.Name.LocalName,
                    attribute.Value, isAttribute: true, LineOf(attribute));
            }

            var children = element.Elements().ToList();

            if (children.Count == 0)
            {
                Add(values, path, element.Name.LocalName, element.Value, isAttribute: false, LineOf(element));
                return;
            }

            foreach (XElement child in children)
            {
                Collect(child, path, values);
            }
        }

        private static void Add(List<XmlValue> values, string path, string name, string value,
                                bool isAttribute, int lineNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            values.Add(new XmlValue
            {
                Path = path,
                Name = name,
                Value = value.Trim(),
                IsAttribute = isAttribute,
                LineNumber = lineNumber
            });
        }

        private static int LineOf(IXmlLineInfo? info)
        {
            return info != null && info.HasLineInfo() ? info.LineNumber : 0;
        }
    }
}
