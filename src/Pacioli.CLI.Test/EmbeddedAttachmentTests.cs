using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice;

namespace Pacioli.CLI.Test
{
    /*
     * An enclosure that stands in the PDF file and not in the XML.
     *
     * A PDF file of the specification can hold an enclosure beside the invoice, for
     * example a bill of quantities or a photograph. The XML names such a file with a URI
     * of the form "#ef=Aufmass.png"; the data of the file stands in the PDF file.
     *
     * Before, the application showed such an attachment as a plain text with the note
     * "reference only", because the descriptor held no data. Now InvoiceWriter takes the
     * data out of the PDF file, thus the application shows a link and the user can open
     * the file.
     */
    [TestFixture]
    public class EmbeddedAttachmentTests
    {
        private static readonly TestSettings Settings = TestSettings.Load();

        /* The sample of the corpus with two enclosures in the PDF file. */
        private static string? SampleWithTwoEnclosures => SampleLocator
            .Find(Settings)
            .FirstOrDefault(sample =>
                sample.FullPath.EndsWith("EN16931_Elektron.pdf", StringComparison.OrdinalIgnoreCase))
            ?.FullPath;

        [Test]
        public void ThePdfFileHoldsTheInvoiceAndTwoOtherFiles()
        {
            string? sample = SampleWithTwoEnclosures;
            if (sample == null)
            {
                Assert.Ignore("The sample EN16931_Elektron.pdf is not on this computer.");
                return;
            }

            var extractor = new EInvoiceExtractor(sample);

            Assert.That(extractor.HasInvoice, Is.True);
            Assert.That(extractor.AttachmentName, Is.EqualTo("factur-x.xml"));
            Assert.That(extractor.EmbeddedFiles, Has.Count.EqualTo(2),
                "The PDF file holds a report and a photograph beside the invoice.");
            Assert.That(extractor.EmbeddedFiles.All(file => file.Data.Length > 0), Is.True);
        }

        /* Each attachment must have its data, thus the application can show a link. */
        [Test]
        public void EachAttachmentOfTheSampleCanBeOpened()
        {
            string? sample = SampleWithTwoEnclosures;
            if (sample == null)
            {
                Assert.Ignore("The sample EN16931_Elektron.pdf is not on this computer.");
                return;
            }

            string output = Path.Combine(Path.GetTempPath(), $"pacioli-{Guid.NewGuid():N}.pdf");

            try
            {
                var writer = new InvoiceWriter(sample);
                writer.Write(output);

                IReadOnlyList<AttachmentDescriptor> attachments = writer.GetAttachments();

                Assert.That(attachments, Has.Count.EqualTo(2));

                foreach (AttachmentDescriptor attachment in attachments)
                {
                    Assert.That(attachment.Data, Is.Not.Null.And.Not.Empty,
                        $"The attachment \"{attachment.Name ?? attachment.FileName}\" has no data, " +
                        "thus the application can show no link for it.");
                    Assert.That(attachment.FileName, Is.Not.Empty);
                }

                /* The two files of this sample: a report and a photograph. */
                var names = attachments.Select(entry => entry.FileName).ToList();
                Assert.That(names.Any(name => name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)), Is.True);
                Assert.That(names.Any(name => name.EndsWith(".png", StringComparison.OrdinalIgnoreCase)), Is.True);
            }
            finally
            {
                if (File.Exists(output)) File.Delete(output);
            }
        }

        /*
         * An XML file alone holds no embedded file. The attachments then come from the
         * XML only, as before.
         */
        [Test]
        public void AnXmlFileGivesTheAttachmentsOfTheXmlOnly()
        {
            SampleFile? sample = SampleLocator
                .Find(Settings)
                .FirstOrDefault(entry =>
                    entry.FullPath.EndsWith("EN16931_Elektron.cii.xml", StringComparison.OrdinalIgnoreCase));

            if (sample == null)
            {
                Assert.Ignore("The sample EN16931_Elektron.cii.xml is not on this computer.");
                return;
            }

            string output = Path.Combine(Path.GetTempPath(), $"pacioli-{Guid.NewGuid():N}.pdf");

            try
            {
                var writer = new InvoiceWriter(sample.FullPath);
                writer.Write(output);

                var attachments = writer.GetAttachments();

                Assert.That(attachments, Has.Count.EqualTo(2), "The XML names two documents.");
                Assert.That(attachments.All(entry => entry.Data == null || entry.Data.Length == 0), Is.True,
                    "The XML holds no data of these files, thus they stay a reference.");
            }
            finally
            {
                if (File.Exists(output)) File.Delete(output);
            }
        }
    }
}
