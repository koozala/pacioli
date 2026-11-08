using En16931;
using En16931.Conformance;
using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using Pacioli.WindowsApp.Util;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Pacioli.WindowsApp.DocumentInfo
{
    /*
     * Makes the HTML page of the document information.
     *
     * The order of the page:
     *   1. The disagreements of the declarations, if the document is a PDF file.
     *   2. Standard and profile (BT-24, BT-23).
     *   3. XML: syntax, root element, namespaces.
     *   4. File: path, size, container, embedded XML file.
     *   5. PDF metadata (XMP), if the document is a PDF file.
     *
     * Each text of the document goes through HtmlPage.Encode.
     */
    public static class DocumentInformationHtml
    {
        const string Style = @"
.issues li { margin: 4px 0; }
";

        public static string Build(DocumentInformation info)
        {
            string fileName = Path.GetFileName(info.FilePath);
            StringBuilder html = HtmlPage.Begin(string.Format(Resources.diTitle, fileName), Style);

            html.Append("<h1>").Append(E(fileName)).Append("</h1>");
            html.Append("<div class=\"meta\">").Append(E(Resources.diDeclaredNote)).Append("</div>");

            AppendIssues(html, info);
            AppendSpecification(html, info);
            AppendXml(html, info);
            AppendFile(html, info);

            if (info.IsPdf)
            {
                AppendXmp(html, info.PdfMetadata);
            }

            return HtmlPage.End(html);
        }

        static void AppendIssues(StringBuilder html, DocumentInformation info)
        {
            /* An XML file has no second declaration, thus nothing to compare. */
            if (!info.IsPdf)
            {
                return;
            }

            if (info.Issues.Count == 0)
            {
                /*
                 * "They agree" is true only if the XMP holds a declaration. Without one,
                 * the section of the XMP metadata tells that it is not there.
                 */
                if (info.PdfMetadata?.InvoiceNamespace != null)
                {
                    html.Append("<div class=\"box ok\">").Append(E(Resources.diNoIssues)).Append("</div>");
                }
                return;
            }

            html.Append("<div class=\"box\"><strong>").Append(E(Resources.diIssuesTitle)).Append("</strong><ul class=\"issues\">");
            foreach (DeclarationIssue issue in info.Issues)
            {
                string format = issue.Rule switch
                {
                    DeclarationRule.ConformanceLevel => Resources.diIssueConformanceLevel,
                    DeclarationRule.DocumentFileName => Resources.diIssueDocumentFileName,
                    DeclarationRule.XmpFamily => Resources.diIssueXmpFamily,
                    _ => Resources.diIssuePdfAPart
                };
                html.Append("<li>").Append(E(string.Format(format, issue.Expected, issue.Actual))).Append("</li>");
            }
            html.Append("</ul></div>");
        }

        static void AppendSpecification(StringBuilder html, DocumentInformation info)
        {
            SpecificationInfo spec = info.Specification;

            html.Append("<h2>").Append(E(Resources.diSpecification)).Append("</h2><table class=\"summary\">");

            if (spec.IsRecognized)
            {
                Row(html, Resources.diStandard, spec.StandardName);
                Row(html, Resources.diProfile, spec.ProfileName);
                Row(html, Resources.diVersion, spec.Version);
            }
            else
            {
                RowRaw(html, Resources.diStandard, $"<span class=\"muted\">{E(Resources.diNotRecognized)}</span>");
            }

            RowCode(html, Resources.diBt24, spec.Identifier);
            RowCode(html, Resources.diBt23, info.BusinessProcessType);
            html.Append("</table>");

            if (spec.Family == SpecificationFamily.En16931)
            {
                html.Append("<p class=\"muted\">").Append(E(Resources.diEn16931Hint)).Append("</p>");
            }
        }

        static void AppendXml(StringBuilder html, DocumentInformation info)
        {
            html.Append("<h2>").Append(E(Resources.diXml)).Append("</h2><table class=\"summary\">");
            Row(html, Resources.diSyntax, SyntaxName(info));
            RowCode(html, Resources.diRootElement, info.RootElement);
            html.Append("</table>");

            if (info.Namespaces.Count > 0)
            {
                html.Append("<h2>").Append(E(Resources.diNamespaces)).Append("</h2>");
                html.Append("<table><tr><th>").Append(E(Resources.diPrefix)).Append("</th><th>")
                    .Append(E(Resources.diNamespace)).Append("</th></tr>");
                foreach (KeyValuePair<string, string> pair in info.Namespaces)
                {
                    html.Append("<tr><td><code>").Append(E(pair.Key.Length == 0 ? "(default)" : pair.Key))
                        .Append("</code></td><td><code>").Append(E(pair.Value)).Append("</code></td></tr>");
                }
                html.Append("</table>");
            }
        }

        static void AppendFile(StringBuilder html, DocumentInformation info)
        {
            html.Append("<h2>").Append(E(Resources.diFile)).Append("</h2><table class=\"summary\">");
            RowCode(html, Resources.diPath, info.FilePath);
            if (info.FileSize != null)
            {
                Row(html, Resources.diSize, info.FileSize.Value.ToString("N0", CultureInfo.CurrentCulture) + " Bytes");
            }
            Row(html, Resources.diContainer, info.IsPdf ? Resources.diContainerPdf : Resources.diContainerXml);
            if (info.IsPdf)
            {
                RowCode(html, Resources.diEmbeddedName, info.EmbeddedXmlName);
            }
            html.Append("</table>");
        }

        static void AppendXmp(StringBuilder html, PdfInvoiceMetadata? metadata)
        {
            html.Append("<h2>").Append(E(Resources.diXmp)).Append("</h2>");

            if (metadata == null || !metadata.HasXmp)
            {
                html.Append("<p class=\"muted\">").Append(E(Resources.diNoXmp)).Append("</p>");
                if (metadata == null)
                {
                    return;
                }
            }

            html.Append("<table class=\"summary\">");

            string pdfA = metadata.PdfAPart != null
                ? $"PDF/A-{metadata.PdfAPart}{metadata.PdfAConformance?.ToLowerInvariant()}"
                : Resources.diNoPdfA;
            Row(html, "PDF/A", pdfA);

            if (metadata.InvoiceNamespace != null)
            {
                string family = SpecificationCatalog.NameOf(DeclarationCheck.FamilyOfXmpNamespace(metadata.InvoiceNamespace)) ?? string.Empty;
                RowRaw(html, Resources.diXmpSchema, $"{E(family)}<br><code>{E(metadata.InvoiceNamespace)}</code>");
                Row(html, "ConformanceLevel", metadata.ConformanceLevel ?? Resources.diNotGiven);
                Row(html, "DocumentType", metadata.DocumentType ?? Resources.diNotGiven);
                Row(html, "DocumentFileName", metadata.DocumentFileName ?? Resources.diNotGiven);
                Row(html, "Version", metadata.Version ?? Resources.diNotGiven);
            }

            Row(html, Resources.diProducer, metadata.Producer);
            Row(html, Resources.diCreationDate, FormatDate(metadata.CreationDate));
            html.Append("</table>");

            if (metadata.HasXmp && metadata.InvoiceNamespace == null)
            {
                html.Append("<p class=\"muted\">").Append(E(Resources.diNoInvoiceXmp)).Append("</p>");
            }
        }

        static string SyntaxName(DocumentInformation info)
        {
            string root = info.RootElement ?? string.Empty;
            string localName = root.Contains(':') ? root.Substring(root.IndexOf(':') + 1) : root;

            return info.Syntax switch
            {
                InvoiceSyntax.Cii => "UN/CEFACT Cross Industry Invoice (CII D16B)",
                InvoiceSyntax.Ubl => $"OASIS UBL 2.1 ({localName})",
                _ => Resources.diNotRecognized
            };
        }

        /*
         * A date of the XMP metadata (ISO 8601) or of the information dictionary
         * ("D:20241112101500+01'00'"), in the form of the current language. A date that
         * the method cannot read stays as it is.
         */
        static string? FormatDate(string? value)
        {
            if (value == null)
            {
                return null;
            }

            if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset iso))
            {
                return iso.ToString("g", CultureInfo.CurrentCulture);
            }

            Match pdf = Regex.Match(value, @"^D:(\d{4})(\d{2})?(\d{2})?(\d{2})?(\d{2})?(\d{2})?");
            if (pdf.Success)
            {
                int Part(int group, int fallback) => pdf.Groups[group].Success ? int.Parse(pdf.Groups[group].Value, CultureInfo.InvariantCulture) : fallback;
                try
                {
                    var date = new DateTime(Part(1, 1), Part(2, 1), Part(3, 1), Part(4, 0), Part(5, 0), Part(6, 0));
                    return date.ToString("g", CultureInfo.CurrentCulture);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return value;
                }
            }

            return value;
        }

        static void Row(StringBuilder html, string name, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            RowRaw(html, name, E(value));
        }

        static void RowCode(StringBuilder html, string name, string? value)
        {
            string cell = string.IsNullOrWhiteSpace(value)
                ? $"<span class=\"muted\">{E(Resources.diNotGiven)}</span>"
                : $"<code>{E(value)}</code>";
            RowRaw(html, name, cell);
        }

        /* The cell must be HTML that the caller encoded. */
        static void RowRaw(StringBuilder html, string name, string cell)
        {
            html.Append("<tr><th>").Append(E(name)).Append("</th><td>").Append(cell).Append("</td></tr>");
        }

        static string E(string? text) => HtmlPage.Encode(text);
    }
}
