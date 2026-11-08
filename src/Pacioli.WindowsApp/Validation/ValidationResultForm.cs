using Pacioli.Language.Resources;
using Pacioli.WindowsApp.Controls;

namespace Pacioli.WindowsApp.Validation
{
    /*
     * The window that shows the result of an online validation.
     *
     * The document keeps the result, not the window. The user can close the window and
     * open it again from the document, as long as the tab of the document is open.
     */
    public sealed class ValidationResultForm : HtmlViewWindow
    {
        public ValidationResultForm(ValidationReport report)
            : base(string.Format(Resources.vrTitle, report.FileName), ValidationReportHtml.Build(report))
        {
        }
    }
}
