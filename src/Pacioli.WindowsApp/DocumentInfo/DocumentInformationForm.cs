using Pacioli.Language.Resources;
using Pacioli.WindowsApp.Controls;

namespace Pacioli.WindowsApp.DocumentInfo
{
    /* The window of the document information. The tab of the document owns the data. */
    public sealed class DocumentInformationForm : HtmlViewWindow
    {
        public DocumentInformationForm(DocumentInformation info)
            : base(string.Format(Resources.diTitle, Path.GetFileName(info.FilePath)), DocumentInformationHtml.Build(info))
        {
        }
    }
}
