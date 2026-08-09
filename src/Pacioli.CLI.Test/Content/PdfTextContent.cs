using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Pacioli.CLI.Test.Content
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

            if (JoinedVisualOrderText.Contains(joined, StringComparison.Ordinal) ||
                JoinedContentOrderText.Contains(joined, StringComparison.Ordinal))
            {
                return true;
            }

            return ContainsInTwoParts(normalized);
        }

        /*
         * The smallest length of a part in the comparison in two parts.
         *
         * A page break can come before the last word of a cell. That word can be short,
         * for example "verdichten" with 10 characters. Thus the length of one part is
         * small. The length of the full value gives the safety: see MinimumValueLength.
         */
        public const int MinimumPartLength = 6;

        /*
         * The smallest length of a value for the comparison in two parts.
         * A short value keeps the strict comparison, thus a weak match cannot occur.
         */
        public const int MinimumValueLength = 24;

        /*
         * Looks for a value that a page break cuts into two parts.
         *
         * A table row can come at the end of a page. The renderer then writes the first
         * part of the cell to that page, and the second part to the next page. Between
         * the two parts come the other columns and the header row of the table again.
         * Thus the value is complete on the page, but not in one part.
         *
         * The test therefore cuts the value at each space, and it looks for the two
         * parts. One cut is sufficient, because one page break makes one cut. Each part
         * must have at least MinimumPartLength characters. Thus a short value keeps the
         * strict comparison, and a weak match cannot occur.
         */
        private bool ContainsInTwoParts(string normalized)
        {
            if (normalized.Length < MinimumValueLength)
            {
                return false;
            }

            for (int index = MinimumPartLength; index < normalized.Length - MinimumPartLength; index++)
            {
                if (normalized[index] != ' ')
                {
                    continue;
                }

                string head = normalized.Substring(0, index);
                string tail = normalized.Substring(index + 1);

                if (ContainsPart(head) && ContainsPart(tail))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsPart(string part)
        {
            if (NormalizedVisualOrderText.Contains(part, StringComparison.Ordinal) ||
                NormalizedContentOrderText.Contains(part, StringComparison.Ordinal))
            {
                return true;
            }

            string joined = JoinBrokenWords(part);

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
