using En16931;
using Pacioli.CLI.Test.Content;
using Pacioli.Pdf.ERechnung;

namespace Pacioli.CLI.Test
{
    /*
     * Takes the invoice out of each PDF sample.
     *
     * Phase 3 of the migration to En16931 gave this step a new form. Before, the reader
     * walked through each object of the PDF file and gave each stream to the parser; a
     * call without an exception was the answer. Now EInvoiceExtractor reads the name tree
     * of the embedded files.
     *
     * The test compares the two ways. The new way must find the same bytes as the old
     * way, for each PDF sample. Thus the new form of the step changes nothing for the
     * user.
     *
     * See doc/En16931-Migration.md, phase 3.
     */
    [TestFixture]
    public class PdfAttachmentTests
    {
        private static readonly TestSettings Settings = TestSettings.Load();

        private static IReadOnlyList<SampleFile> PdfSamples { get; } = SampleLocator
            .Find(Settings)
            .Where(sample => sample.FullPath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            .ToList();

        private static IEnumerable<TestCaseData> PdfCases()
        {
            if (PdfSamples.Count == 0)
            {
                yield return new TestCaseData((SampleFile?)null).SetName("no PDF sample");
                yield break;
            }

            foreach (SampleFile sample in PdfSamples)
            {
                yield return new TestCaseData(sample).SetName(sample.RelativePath);
            }
        }

        /*
         * The new way and the old way must give the same bytes.
         *
         * If both ways find nothing, the sample holds no invoice, and the case passes:
         * the corpus holds a PDF file without an attachment.
         */
        [TestCaseSource(nameof(PdfCases))]
        public void Sample_NewWayFindsTheSameInvoiceAsTheOldWay(SampleFile? sample)
        {
            if (sample == null)
            {
                Assert.Ignore("The samples directories hold no PDF file.");
                return;
            }

            byte[]? old = LegacyPdfAttachmentReader.Read(sample.FullPath);
            byte[]? current = BytesOf(new EInvoiceExtractor(sample.FullPath));

            if (old == null && current == null)
            {
                Assert.Pass("No way finds an invoice in this file.");
                return;
            }

            Assert.That(current, Is.Not.Null,
                "The old way finds an invoice, the new way finds none. " +
                $"The old way gives {old!.Length} byte(s).");

            Assert.That(old, Is.Not.Null,
                "The new way finds an invoice that the old way does not find. " +
                "This can be correct for a UBL invoice, because the old library reads CII only. " +
                $"The new way gives {current!.Length} byte(s).");

            Assert.That(current, Is.EqualTo(old),
                $"The two ways give different bytes: old {old!.Length}, new {current!.Length}.");
        }

        /* Each invoice that the new way finds must give a syntax and a name. */
        [TestCaseSource(nameof(PdfCases))]
        public void Sample_TheExtractorNamesTheAttachmentAndTheSyntax(SampleFile? sample)
        {
            if (sample == null)
            {
                Assert.Ignore("The samples directories hold no PDF file.");
                return;
            }

            var extractor = new EInvoiceExtractor(sample.FullPath);

            if (!extractor.HasInvoice)
            {
                Assert.Pass("The file holds no invoice.");
                return;
            }

            Assert.That(extractor.Syntax, Is.Not.EqualTo(InvoiceSyntax.Unknown));
            Assert.That(extractor.AttachmentName, Is.Not.Null.And.Not.Empty,
                "The attachment has no file name. The specification asks for one.");
            Assert.That(extractor.XmlDataStream!.Position, Is.Zero,
                "The stream must stand at the beginning, because the caller reads it directly.");
        }

        /* A file that is not a PDF file must not raise an exception. */
        [Test]
        public void AFileThatIsNotAPdfFile_GivesNoInvoice()
        {
            string path = Path.Combine(Path.GetTempPath(), $"pacioli-{Guid.NewGuid():N}.pdf");
            File.WriteAllText(path, "This is not a PDF file.");

            try
            {
                var extractor = new EInvoiceExtractor(path);

                Assert.That(extractor.HasInvoice, Is.False);
                Assert.That(extractor.Syntax, Is.EqualTo(InvoiceSyntax.Unknown));
                Assert.That(extractor.XmlDataStream, Is.Null);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void AFileThatDoesNotExist_GivesNoInvoice()
        {
            var extractor = new EInvoiceExtractor(
                Path.Combine(Path.GetTempPath(), $"no-such-file-{Guid.NewGuid():N}.pdf"));

            Assert.That(extractor.HasInvoice, Is.False);
        }

        private static byte[]? BytesOf(EInvoiceExtractor extractor)
        {
            if (!extractor.HasInvoice)
            {
                return null;
            }

            using var memory = new MemoryStream();
            extractor.XmlDataStream!.CopyTo(memory);
            return memory.ToArray();
        }
    }
}
