using System.Net;
using System.Text;

namespace Pacioli.WindowsApp.Util
{
    /*
     * The frame of an HTML page of the application: the head with the
     * Content-Security-Policy, and the style sheet that all pages share.
     *
     * The policy permits only the style sheet of the page: no script, no image, and no
     * other file.
     */
    public static class HtmlPage
    {
        const string BaseStyle = @"
:root { color-scheme: light; --ok: #1e7b3a; --bad: #b3261e; --warn: #8a5a00; --muted: #5f6368; --line: #dadce0; --head: #f1f3f4; }
body { font-family: 'Segoe UI', sans-serif; font-size: 14px; color: #202124; background: #ffffff; margin: 20px; }
h1 { font-size: 20px; margin: 0 0 4px 0; }
h2 { font-size: 16px; margin: 24px 0 8px 0; }
.meta { color: var(--muted); margin-bottom: 16px; }
table { border-collapse: collapse; width: 100%; }
th, td { text-align: left; vertical-align: top; padding: 6px 8px; border-bottom: 1px solid var(--line); }
th { background: var(--head); font-weight: 600; }
table.summary { width: auto; }
table.summary th { background: transparent; font-weight: normal; color: var(--muted); padding-right: 24px; white-space: nowrap; }
code { font-family: Consolas, monospace; font-size: 12px; word-break: break-all; }
.box { border-left: 4px solid var(--warn); background: #fff8e1; padding: 8px 12px; }
.box.ok { border-left-color: var(--ok); background: #e6f4ea; }
.muted { color: var(--muted); }
.small { color: var(--muted); font-size: 12px; margin-top: 24px; }
";

        /* Starts a page. The caller adds the body and then calls End. */
        public static StringBuilder Begin(string title, string extraStyle = "")
        {
            var html = new StringBuilder();
            html.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\">");
            html.Append("<meta http-equiv=\"Content-Security-Policy\" content=\"default-src 'none'; style-src 'unsafe-inline'\">");
            html.Append("<title>").Append(Encode(title)).Append("</title>");
            html.Append("<style>").Append(BaseStyle).Append(extraStyle).Append("</style></head><body>");
            return html;
        }

        public static string End(StringBuilder html)
        {
            html.Append("</body></html>");
            return html.ToString();
        }

        /* Each text that goes into a page goes through this method. */
        public static string Encode(string? text)
        {
            return WebUtility.HtmlEncode(text ?? string.Empty);
        }
    }
}
