using En16931.Model;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using System.Collections.Generic;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The attached documents (BG-24).
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
            foreach (SupportingDocument document in context.Core.SupportingDocuments)
            {
                AddAttachment(doc, context, document);
            }

            foreach (InvoiceLine line in context.Core.Lines)
            {
                foreach (SupportingDocument document in line.Extensions?.Documents
                                                        ?? (IReadOnlyList<SupportingDocument>)new List<SupportingDocument>())
                {
                    AddAttachment(doc, context, document);
                }
            }
        }

        /* Writes one line for an attached document, and keeps the attachment for the caller. */
        private static void AddAttachment(Document doc, InvoiceContext context, SupportingDocument document)
        {
            Attachment? attachment = document.Attachment;

            doc.Add(new Paragraph($"{Resources.attachment}: {document.Description} {attachment?.FileName}"));

            context.AddAttachment(new AttachmentDescriptor
            {
                FileName = attachment?.FileName ?? string.Empty,
                Name = document.Description,
                ReferenceId = document.Reference,

                /* BT-124. InvoiceWriter takes the data out of the PDF file with it. */
                Uri = document.ExternalLocation,

                Data = attachment?.Content,
                MimeType = attachment?.MimeCode
            });
        }
    }
}
