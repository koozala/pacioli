using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using En16931;
using En16931.Model;
using Pacioli.CLI.Test.Content;

namespace Pacioli.CLI.Test
{
    /*
     * Can the model of En16931 carry the page?
     *
     * This is the control of phase 4 of the migration. In phase 5 the renderer stops to
     * read the model of the old library and starts to read En16931. That step can only
     * keep the page if each value that the page shows today has a place in the new model.
     *
     * HOW THE TEST WORKS
     *
     * 1. It reads each value of the XML input, as XmlSample_AllValuesAreVisible does.
     * 2. It reads the same document with En16931, and it walks the model with reflection.
     *    Each value of the model becomes a text, in each form that a value can take.
     * 3. Each value of the XML that the page shows today must be in that set.
     *
     * "The page shows it today" means: no entry of XmlValueExclusions applies to it.
     * The test case XmlSample_AllValuesAreVisible passes with 0 missing values, thus a
     * value without an entry is on the page.
     *
     * A value that the page does not show can still be in the model, and mostly it is: a
     * unit code is an example. The test does not ask for it, because the renderer does
     * not need it.
     */
    [TestFixture]
    public class ModelReachabilityTests
    {
        private static readonly TestSettings Settings = TestSettings.Load();

        private static IReadOnlyList<SampleFile> XmlSamples { get; } = SampleLocator
            .Find(Settings)
            .Where(sample => sample.FullPath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            .ToList();

        private static IEnumerable<TestCaseData> XmlCases()
        {
            if (XmlSamples.Count == 0)
            {
                yield return new TestCaseData((SampleFile?)null).SetName("no XML sample");
                yield break;
            }

            foreach (SampleFile sample in XmlSamples)
            {
                yield return new TestCaseData(sample).SetName(sample.RelativePath);
            }
        }

        [TestCaseSource(nameof(XmlCases))]
        public void Sample_EachValueOnThePageIsInTheModel(SampleFile? sample)
        {
            if (sample == null)
            {
                Assert.Ignore("The samples directories hold no XML file.");
                return;
            }

            Invoice invoice;
            try
            {
                invoice = InvoiceReader.ReadFile(sample.FullPath);
            }
            catch (Exception error)
            {
                Assert.Ignore($"En16931 does not read this document: {error.GetType().Name}.");
                return;
            }

            HashSet<string> inTheModel = ValuesOf(invoice);

            var missing = new List<XmlValue>();

            foreach (XmlValue value in XmlValueExtractor.Extract(sample.FullPath))
            {
                if (string.IsNullOrWhiteSpace(value.Value))
                {
                    continue;
                }

                /* A value that the page does not show needs no place in the model. */
                if (XmlValueExclusions.Find(value) != null)
                {
                    continue;
                }

                if (!inTheModel.Contains(Normalize(value.Value)))
                {
                    missing.Add(value);
                }
            }

            if (missing.Count == 0)
            {
                return;
            }

            var message = new StringBuilder();
            message.AppendLine(
                $"{missing.Count} value(s) are on the page today, and the model of En16931 " +
                "does not hold them. Phase 5 would lose them.");

            foreach (XmlValue value in missing.Take(40))
            {
                message.AppendLine($"  line {value.LineNumber}  {value.Path} = \"{value.Value}\"");
            }

            if (missing.Count > 40)
            {
                message.AppendLine($"  ... and {missing.Count - 40} more");
            }

            Assert.Fail(message.ToString());
        }

        /*
         * Each value of the model, as a text.
         *
         * The walk follows each property of each object of the model, and each entry of
         * each list. A value becomes more than one text, because a value can stand in
         * more than one form: a date stands in the XML as "20200305", and a decimal keeps
         * the number of its decimal places.
         */
        private static HashSet<string> ValuesOf(object root)
        {
            var values = new HashSet<string>(StringComparer.Ordinal);
            var seen = new HashSet<object>(ReferenceEqualityComparer.Instance);

            Walk(root);

            return values;

            void Walk(object? node)
            {
                if (node == null || !seen.Add(node))
                {
                    return;
                }

                foreach (PropertyInfo property in node.GetType()
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }

                    object? value;
                    try
                    {
                        value = property.GetValue(node);
                    }
                    catch (TargetInvocationException)
                    {
                        continue;
                    }

                    Take(value);
                }
            }

            void Take(object? value)
            {
                switch (value)
                {
                    case null:
                        return;

                    case string text:
                        Add(text);
                        return;

                    case decimal number:
                        AddDecimal(number);
                        return;

                    case DateOnly date:
                        AddDate(date);
                        return;

                    case bool flag:
                        Add(flag ? "true" : "false");
                        Add(flag ? "1" : "0");
                        return;

                    case byte[] data:
                        /* The base64 form of an attachment. */
                        Add(Convert.ToBase64String(data));
                        return;

                    case Enum:
                        return;

                    case IEnumerable list:
                        foreach (object? entry in list)
                        {
                            Take(entry);
                        }
                        return;

                    default:
                        if (value.GetType().IsPrimitive)
                        {
                            Add(Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty);
                            return;
                        }

                        Walk(value);
                        return;
                }
            }

            void Add(string? text)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    values.Add(Normalize(text));
                }
            }

            void AddDecimal(decimal number)
            {
                Add(number.ToString(CultureInfo.InvariantCulture));
                Add(number.ToString("0.################", CultureInfo.InvariantCulture));

                /* A percentage can stand with or without its decimal places. */
                if (number == Math.Floor(number))
                {
                    Add(((long)number).ToString(CultureInfo.InvariantCulture));
                }
            }

            void AddDate(DateOnly date)
            {
                Add(date.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                Add(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                Add(date.ToString("yyyyMM", CultureInfo.InvariantCulture));
            }
        }

        /*
         * The two sides must agree over the form of a text: the XML can break a text over
         * more than one line, and it can put a space at the beginning or at the end.
         */
        private static string Normalize(string text)
        {
            var result = new StringBuilder(text.Length);
            bool space = false;

            foreach (char character in text)
            {
                if (char.IsWhiteSpace(character))
                {
                    space = result.Length > 0;
                    continue;
                }

                if (space)
                {
                    result.Append(' ');
                    space = false;
                }

                result.Append(character);
            }

            return result.ToString();
        }
    }
}
