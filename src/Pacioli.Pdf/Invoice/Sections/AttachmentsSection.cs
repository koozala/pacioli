using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using System.Collections.Generic;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The attached documents.
     *
     * The section writes one line for each attachment, and it keeps the data of the
     * attachment in the context. InvoiceWriter.GetAttachments then gives the list to the
     * application, and the application can open an attachment.
     *
     * An attachment comes from the document itself, or from one position.
     */
    internal sealed class AttachmentsSection : IInvoiceSection
    {
        public string Name => "Attachments";

        public void Write(Document doc, InvoiceContext context)
        {
            foreach (var refdoc in context.Agreement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
            {
                AddAttachment(doc, context, refdoc);
            }

            foreach (var item in context.LineItems)
            {
                foreach (var refdoc in item.SpecifiedLineTradeSettlement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
                {
                    AddAttachment(doc, context, refdoc);
                }
                foreach (var refdoc in item.SpecifiedLineTradeAgreement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
                {
                    AddAttachment(doc, context, refdoc);
                }
            }
        }

        /* Writes one line for an attached document, and keeps the attachment for the caller. */
        private static void AddAttachment(Document doc, InvoiceContext context, ReferencedDocument refdoc)
        {
            BinaryObject? binary = refdoc.AttachmentBinaryObject;

            doc.Add(new Paragraph($"{Resources.attachment}: {refdoc.Name} {binary?.FileName}"));

            context.AddAttachment(new AttachmentDescriptor
            {
                FileName = binary?.FileName ?? string.Empty,
                Name = refdoc.Name,
                Data = binary?.Value,
                MimeType = binary?.MimeCode
            });
        }
    }
}
