using En16931.Model;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System;
using System.Linq;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The date of the delivery or of the service (Liefer- oder Leistungsdatum).
     *
     * German VAT law (§ 14 Abs. 4 Satz 1 Nr. 6 UStG) asks for the date of the delivery or
     * of the service on each invoice. EN 16931 has two places for it at the level of the
     * document:
     *
     *   BT-72   Actual delivery date (one day).
     *   BG-14   Invoicing period (BT-73 start, BT-74 end).
     *
     * A position can also give its own delivery date or period (BG-26).
     *
     * The box is ALWAYS on the page, because a missing date is also information for the
     * reader. It shows one of these:
     *
     *   1. The delivery date and/or the invoicing period of the document.
     *   2. "In the positions", if only the positions give a date.
     *   3. "Not given", if the document gives no date.
     *
     * The boxes of the delivery and of the settlement (TradeAgreementSection) do not show
     * these dates again.
     */
    internal sealed class ServiceDateSection : IInvoiceSection
    {
        public string Name => "Service Date";

        static readonly Color FrameColor = new DeviceRgb(0x8a, 0x5a, 0x00);

        public void Write(Document doc, InvoiceContext context)
        {
            var invoice = context.Core;

            Div box = new Div()
                .SetBorder(new SolidBorder(FrameColor, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);

            box.Add(new Paragraph(Resources.LieferLeistungsdatum).SetFont(context.Bold).SetFontSize(12));

            DateOnly? deliveryDate = invoice.Delivery?.ActualDeliveryDate;
            string? period = FormatPeriod(invoice.InvoicingPeriod);

            if (deliveryDate != null)
            {
                box.Add(new Paragraph($"{Resources.Lieferdatum}: {deliveryDate.Value.ToString("d")}").SetFontSize(12));
            }

            if (period != null)
            {
                box.Add(new Paragraph($"{Resources.billingPeriod}: {period}").SetFontSize(12));
            }

            if (deliveryDate == null && period == null)
            {
                string text = LinesHaveDates(invoice)
                    ? Resources.LeistungsdatumInPositionen
                    : Resources.LeistungsdatumNichtAngegeben;

                box.Add(new Paragraph(text).SetFontSize(12).SetFontColor(ColorConstants.DARK_GRAY));
            }

            doc.Add(box);
        }

        /*
         * The period in the form "01.10.2024 - 31.10.2024". A period with one end only
         * shows that end. A period without a date gives null.
         */
        internal static string? FormatPeriod(Period? period)
        {
            string? start = period?.StartDate?.ToString("d");
            string? end = period?.EndDate?.ToString("d");

            if (start == null && end == null)
            {
                return null;
            }

            return $"{start ?? "…"} - {end ?? "…"}";
        }

        /* True if a position gives its own delivery date or period. */
        static bool LinesHaveDates(En16931.Model.Invoice invoice)
        {
            return invoice.Lines.Any(line =>
                line.Extensions?.ActualDeliveryDate != null
                || line.Period?.StartDate != null
                || line.Period?.EndDate != null);
        }
    }
}
