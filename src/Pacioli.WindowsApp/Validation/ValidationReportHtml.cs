using Pacioli.Language.Resources;
using Pacioli.WindowsApp.Util;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Pacioli.WindowsApp.Validation
{
    /*
     * Makes the HTML page of a validation result.
     *
     * Each text of the service goes through HtmlEncode, because the program does not
     * control it. The page has no script, and the Content-Security-Policy permits only
     * the style sheet of the page.
     */
    public static class ValidationReportHtml
    {
        /* The style of this page. HtmlPage gives the style that all pages share. */
        const string Style = @"
.badge { display: inline-block; padding: 4px 12px; border-radius: 4px; color: #ffffff; font-weight: 600; margin-right: 8px; }
.badge.ok { background: var(--ok); }
.badge.bad { background: var(--bad); }
td.rule { white-space: nowrap; font-weight: 600; }
td.rule .set { display: block; font-weight: normal; color: var(--muted); font-size: 12px; }
.error td.rule { color: var(--bad); }
.warning td.rule { color: var(--warn); }
.fix { color: var(--muted); margin-top: 4px; }
.vat { color: var(--muted); font-size: 12px; margin-top: 4px; }
";

        public static string Build(ValidationReport report)
        {
            StringBuilder html = HtmlPage.Begin(string.Format(Resources.vrTitle, report.FileName), Style);

            html.Append("<h1>")
                .Append(report.Valid
                    ? $"<span class=\"badge ok\">{E(Resources.vrValid)}</span>"
                    : $"<span class=\"badge bad\">{E(Resources.vrInvalid)}</span>")
                .Append(E(report.FileName))
                .Append("</h1>");
            html.Append("<div class=\"meta\">")
                .Append(E(string.Format(Resources.vrCheckedAt, report.CheckedAt.ToString("g", CultureInfo.CurrentCulture))))
                .Append("</div>");

            AppendSummary(html, report);

            if (report.Germany != null && (report.Germany.Status != null || report.Germany.Explanation != null))
            {
                html.Append("<h2>").Append(E(Resources.vrGermany)).Append("</h2><div class=\"box\">");
                if (report.Germany.Status != null)
                {
                    html.Append("<code>").Append(E(report.Germany.Status)).Append("</code><br>");
                }
                html.Append(E(report.Germany.Explanation)).Append("</div>");
            }

            html.Append("<h2>").Append(E(Resources.vrErrors)).Append($" ({report.Errors.Count})</h2>");
            AppendFindings(html, report.Errors, "error", Resources.vrNoErrors);

            html.Append("<h2>").Append(E(Resources.vrWarnings)).Append($" ({report.Warnings.Count})</h2>");
            AppendFindings(html, report.Warnings, "warning", Resources.vrNoWarnings);

            if (report.Notes.Count > 0)
            {
                html.Append("<h2>").Append(E(Resources.vrNotes)).Append("</h2><ul>");
                foreach (string note in report.Notes)
                {
                    html.Append("<li>").Append(E(note)).Append("</li>");
                }
                html.Append("</ul>");
            }

            if (report.Disclaimer != null)
            {
                html.Append("<div class=\"small\">").Append(E(report.Disclaimer)).Append("</div>");
            }

            return HtmlPage.End(html);
        }

        static void AppendSummary(StringBuilder html, ValidationReport report)
        {
            html.Append("<table class=\"summary\">");
            Row(html, Resources.vrErrors, report.ErrorCount.ToString(CultureInfo.CurrentCulture));
            Row(html, Resources.vrWarnings, report.WarningCount.ToString(CultureInfo.CurrentCulture));
            if (report.FormatErrorCount != null)
            {
                Row(html, Resources.vrFormatErrors, report.FormatErrorCount.Value.ToString(CultureInfo.CurrentCulture));
            }
            if (report.BusinessRuleErrorCount != null)
            {
                Row(html, Resources.vrBusinessRuleErrors, report.BusinessRuleErrorCount.Value.ToString(CultureInfo.CurrentCulture));
            }

            string syntax = string.Join(" / ", new[] { report.Syntax?.ToUpperInvariant(), report.Container?.ToUpperInvariant() }
                .Where(text => !string.IsNullOrEmpty(text)));
            Row(html, Resources.vrSyntax, syntax);
            Row(html, Resources.vrProfile, report.Profile);
            if (report.CustomizationId != null)
            {
                html.Append("<tr><th>CustomizationID</th><td><code>").Append(E(report.CustomizationId)).Append("</code></td></tr>");
            }

            string checkedSets = string.Join(", ", report.Checked.Where(pair => pair.Value).Select(pair => pair.Key));
            Row(html, Resources.vrChecked, checkedSets);

            string versions = string.Join(", ", report.RuleVersions.Select(pair => $"{pair.Key} {pair.Value}"));
            Row(html, Resources.vrRuleVersions, versions);
            html.Append("</table>");
        }

        static void Row(StringBuilder html, string name, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            html.Append("<tr><th>").Append(E(name)).Append("</th><td>").Append(E(value)).Append("</td></tr>");
        }

        static void AppendFindings(StringBuilder html, IReadOnlyList<ValidationFinding> findings, string cssClass, string emptyText)
        {
            if (findings.Count == 0)
            {
                html.Append("<p>").Append(E(emptyText)).Append("</p>");
                return;
            }

            html.Append("<table><tr><th>").Append(E(Resources.vrRule))
                .Append("</th><th>").Append(E(Resources.vrMessage))
                .Append("</th><th>").Append(E(Resources.vrBusinessTerms))
                .Append("</th><th>").Append(E(Resources.vrLocation))
                .Append("</th></tr>");

            foreach (ValidationFinding finding in findings)
            {
                html.Append($"<tr class=\"{cssClass}\"><td class=\"rule\">").Append(E(finding.RuleId));
                if (finding.Ruleset != null)
                {
                    html.Append("<span class=\"set\">").Append(E(finding.Ruleset)).Append("</span>");
                }
                html.Append("</td><td>").Append(E(finding.Message));

                if (finding.Fix != null)
                {
                    html.Append("<div class=\"fix\">").Append(E(Resources.vrFix)).Append(": ").Append(E(finding.Fix)).Append("</div>");
                }

                foreach (GermanVatItem vat in finding.GermanVatItems)
                {
                    html.Append("<div class=\"vat\">").Append(E(vat.Provision));
                    if (vat.Item != null)
                    {
                        html.Append(" – ").Append(E(vat.Item));
                    }
                    html.Append("</div>");
                }

                html.Append("</td><td>").Append(E(string.Join(", ", finding.BusinessTerms)))
                    .Append("</td><td><code title=\"").Append(E(finding.Location)).Append("\">")
                    .Append(E(ShortLocation(finding.Location))).Append("</code></td></tr>");
            }

            html.Append("</table>");
        }

        /*
         * The XPath without the names of the namespaces. The service writes each step as
         * "Q{urn:...}Invoice[1]". The short form "/Invoice[1]" is easier to read. The
         * full path stays in the tooltip.
         */
        static string? ShortLocation(string? location)
        {
            return location == null ? null : Regex.Replace(location, @"Q\{[^}]*\}", string.Empty);
        }

        static string E(string? text) => HtmlPage.Encode(text);
    }
}
