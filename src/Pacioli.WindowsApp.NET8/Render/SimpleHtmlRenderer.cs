using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pacioli.WindowsApp.NET8.Render
{
    public class SimpleHtmlRenderer
    {
        private string HtmlFileName;
        private string OriginalFileName;

        public SimpleHtmlRenderer()
        {
            HtmlFileName = Path.GetTempFileName()+".html";
        }

        public string GetHtmlFileName()
        {
            return HtmlFileName;
        }

        public void Render(byte[] data, string originalFileName)
        {
            string fileBaseName = Path.GetFileName(originalFileName);
            MemoryStream memoryStream = new MemoryStream(data);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memoryStream);

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<!DOCTYPE html>");
            htmlBuilder.AppendLine("<html>");
            htmlBuilder.AppendLine($"<head><title>{fileBaseName}</title></head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("<ul>");

            ConvertXmlNodeToHtml(xmlDoc.DocumentElement, htmlBuilder);

            htmlBuilder.AppendLine("</ul>");
            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");

            System.IO.File.WriteAllText(HtmlFileName, htmlBuilder.ToString());
        }

        public void Render(string originalFileName)
        {
            string fileBaseName = Path.GetFileName(originalFileName);

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(originalFileName);

                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine($"<head><title>{fileBaseName}</title></head>");
                htmlBuilder.AppendLine("<body>");
                htmlBuilder.AppendLine("<ul>");

                ConvertXmlNodeToHtml(xmlDoc.DocumentElement, htmlBuilder);

                htmlBuilder.AppendLine("</ul>");
                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                System.IO.File.WriteAllText(HtmlFileName, htmlBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        void ConvertXmlNodeToHtml(XmlNode node, StringBuilder htmlBuilder)
        {
            if (node == null) return;

            htmlBuilder.AppendLine("<li>");

            var firstChild = node.FirstChild;
            if (firstChild != null && firstChild.NodeType == XmlNodeType.Text)
            {
                htmlBuilder.AppendLine($"<strong>{node.Name}:</strong> {node.InnerText}");
                return;
            }

            htmlBuilder.AppendLine($"<strong>{node.Name}:</strong>");

            htmlBuilder.AppendLine("<ul>");
            foreach (XmlNode childNode in node.ChildNodes)
            {
                ConvertXmlNodeToHtml(childNode, htmlBuilder);
            }
            htmlBuilder.AppendLine("</ul>");

            htmlBuilder.AppendLine("</li>");
        }
    }
}
