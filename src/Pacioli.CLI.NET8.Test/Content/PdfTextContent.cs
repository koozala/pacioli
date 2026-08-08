using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Pacioli.CLI.NET8.Test.Content
{
    /*
     * The text of a rendered PDF file.
     *
     * The class reads the text two times, because one order is not sufficient:
     *
     *   Visual order (LocationTextExtractionStrategy)
     *     The text comes in the order of the page. This order is correct for a
     *     paragraph. But in a table, it mixes the columns: if the text of a cell
     *     goes to a second line, the text of the other columns comes between the
     *     two lines of that cell.
     *
     *   Content order (SimpleTextExtractionStrategy)
     *     The text comes in the order of the PDF content. The renderer writes each
     *     table cell as one block. Thus the text of a cell stays together.
     *
     * A value is visible if one of the two orders holds it.
     *
     * The renderer also breaks a long value into more than one line. Thus the class
     * keeps a normalized form of each text: it replaces each group of space characters
     * with one space. The search uses these forms.
     *
     * A line break after a hyphen or a slash needs one more step. The renderer can break
     * "Brechsand-Splitt-Gemisch" into "Brechsand-Splitt-" and "Gemisch". The text is
     * complete, but it now holds a space that the value does not hold. Thus the class
     * keeps a second form: it also removes each space that comes directly after a hyphen
     * or a slash. The search uses this form if the first form gives no result. The test
     * applies the same step to the value, so the two sides stay equal.
     */
    public sealed class PdfTextContent
    {
        private PdfTextContent(string visualOrderText, string contentOrderText, int pageCount)
        {
            VisualOrderText = visualOrderText;
            ContentOrderText = contentOrderText;
            NormalizedVisualOrderText = Normalize(visualOrderText);
            NormalizedContentOrderText = Normalize(contentOrderText);
            JoinedVisualOrderText = JoinBrokenWords(NormalizedVisualOrderText);
            JoinedContentOrderText = JoinBrokenWords(NormalizedContentOrderText);
            PageCount = pageCount;
        }

        public string VisualOrderText { get; }
        public string ContentOrderText { get; }
        public string NormalizedVisualOrderText { get; }
        public string NormalizedContentOrderText { get; }
        public string JoinedVisualOrderText { get; }
        public string JoinedContentOrderText { get; }
        public int PageCount { get; }

        /* Reads the text of all pages in the two orders. */
        public static PdfTextContent Read(string pdfPath)
        {
            var visualOrder = new StringBuilder();
            var contentOrder = new StringBuilder();

            using var reader = new PdfReader(pdfPath);
            using var document = new PdfDocument(reader);

            int pageCount = document.GetNumberOfPages();

            for (int page = 1; page <= pageCount; page++)
            {
                PdfPage currentPage = document.GetPage(page);
                visualOrder.AppendLine(PdfTextExtractor.GetTextFromPage(currentPage, new LocationTextExtractionStrategy()));
                contentOrder.AppendLine(PdfTextExtractor.GetTextFromPage(currentPage, new SimpleTextExtractionStrategy()));
            }

            return new PdfTextContent(visualOrder.ToString(), contentOrder.ToString(), pageCount);
        }

        public bool Contains(string value)
        {
            string normalized = Normalize(value);

            if (normalized.Length == 0)
            {
                return false;
            }

            if (NormalizedVisualOrderText.Contains(normalized, StringComparison.Ordinal) ||
                NormalizedContentOrderText.Contains(normalized, StringComparison.Ordinal))
            {
                return true;
            }

            /* The renderer can break the value after a hyphen or a slash. */
            string joined = JoinBrokenWords(normalized);

            return JoinedVisualOrderText.Contains(joined, StringComparison.Ordinal) ||
                   JoinedContentOrderText.Contains(joined, StringComparison.Ordinal);
        }

        public bool ContainsAny(IEnumerable<string> values) => values.Any(Contains);

        /* Replaces each group of space characters with one space, and removes the outer spaces. */
        public static string Normalize(string text)
        {
            var result = new StringBuilder(text.Length);
            bool lastWasSpace = false;

            foreach (char character in text)
            {
                if (char.IsWhiteSpace(character))
                {
                    if (!lastWasSpace)
                    {
                        result.Append(' ');
                        lastWasSpace = true;
                    }
                }
                else
                {
                    result.Append(character);
                    lastWasSpace = false;
                }
            }

            return result.ToString().Trim();
        }

        /*
         * Removes each space that comes directly after a hyphen or a slash.
         * The renderer can break a line at these characters. The break adds a space.
         * Give a normalized text to this method.
         */
        public static string JoinBrokenWords(string text)
        {
            var result = new StringBuilder(text.Length);

            for (int index = 0; index < text.Length; index++)
            {
                if (text[index] == ' ' && result.Length > 0 &&
                    (result[^1] == '-' || result[^1] == '/'))
                {
                    continue;
                }

                result.Append(text[index]);
            }

            return result.ToString();
        }
    }
}
