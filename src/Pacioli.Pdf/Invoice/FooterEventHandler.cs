using iText.Commons.Actions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Element;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Writes a paragraph at the foot of each page.
     *
     * Note: no code calls this class at the moment. It was in InvoiceWriter.cs, for a
     * footer with the registration note. The refactoring of InvoiceWriter kept it here.
     */
    class FooterEventHandler : IEventHandler
    {
        float _leftMargin, _bottomMargin, _rightMargin, _pageWidth;
        Paragraph _para;

        public FooterEventHandler(float leftMargin, float bottomMargin, float rightMargin, float pageWidth, Paragraph para)
        {
            _leftMargin = leftMargin;
            _bottomMargin = bottomMargin;
            _rightMargin = rightMargin;
            _pageWidth = pageWidth;
            _para = para;
        }

        public void HandleEvent(IEvent ev)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)ev;
            PdfDocument doc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas cv = new PdfCanvas(page);

            var c = new Canvas(cv, new iText.Kernel.Geom.Rectangle(_leftMargin, _bottomMargin, _pageWidth - _leftMargin - _rightMargin, 50.0f));
            c.Add(_para);
        }

        public void OnEvent(IEvent @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument doc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas cv = new PdfCanvas(page);

            var c = new Canvas(cv, new iText.Kernel.Geom.Rectangle(_leftMargin, _bottomMargin, _pageWidth - _leftMargin - _rightMargin, 50.0f));
            c.Add(_para);
        }
    }
}
