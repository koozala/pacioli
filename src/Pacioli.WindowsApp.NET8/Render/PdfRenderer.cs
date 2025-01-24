using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Render
{
    public class PdfRenderer
    {
        private string pdfPath;
        private string aInfo = Resources.attachmentsNone;
        private SimpleHtmlRenderer simple = new SimpleHtmlRenderer();

        public PdfRenderer()
        {
        }

        public void Render(string originalFileName, string attachmentFolder)
        {
            pdfPath = Path.GetTempFileName();
            Writer writer = new Writer(originalFileName, attachmentFolder);
            writer.Write(pdfPath);
            int countAttachments = writer.GetAttachmentCount();
            if (countAttachments > 0)
            {
                aInfo = string.Format(Resources.attachmentsTxt, countAttachments);
            }

            if (writer.IsZugferd)
            {
                simple.Render(writer.XmlData!, originalFileName);
            }
            else
            {
                 simple.Render(originalFileName);
            }
        }

        public string GetPdfPath()
        {
            return pdfPath;
        }

        public string GetAttachmentInfo()
        {
            return aInfo;
        }

        public string GetHtmlPath()
        {
            return simple.GetHtmlFileName();
        }
    }
}
