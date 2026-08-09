using System.Globalization;
using System.Text;

namespace Pacioli.CLI.Test.Content
{
    /*
     * Collects the result of the test case XmlSample_AllValuesAreVisible, and writes it
     * to a file in Markdown form.
     *
     * The file gives an overview of the values that the renderer does not show. Use it to
     * decide which value the renderer must show in the future.
     */
    public sealed class XmlCoverageReport
    {
        private const int MaximumExamples = 5;

        private readonly List<SampleResult> _samples = new List<SampleResult>();
        private readonly Dictionary<string, GapGroup> _gaps = new Dictionary<string, GapGroup>();
        private readonly object _lock = new object();

        public string SamplesDirectory { get; set; } = string.Empty;
        public string CliExecutable { get; set; } = string.Empty;

        public int SampleCount
        {
            get { lock (_lock) { return _samples.Count; } }
        }

        /*
         * Takes the result of one sample: how many values are on the page, and each
         * value that is not. The counts of the kinds come from the second list, thus
         * the caller cannot give a count that does not agree with the list.
         */
        public void AddSample(string sample, int found, IReadOnlyList<NotOnPageValue> notOnPage)
        {
            lock (_lock)
            {
                _samples.Add(new SampleResult
                {
                    Name = sample,
                    Found = found,
                    NotOnPage = notOnPage.ToList()
                });
            }
        }

        public void AddKnownGap(string sample, XmlValueExclusion exclusion, XmlValue value)
        {
            lock (_lock)
            {
                if (!_gaps.TryGetValue(exclusion.Pattern, out GapGroup? group))
                {
                    group = new GapGroup { Exclusion = exclusion };
                    _gaps[exclusion.Pattern] = group;
                }

                group.Count++;
                group.Samples.Add(sample);

                if (group.Examples.Count < MaximumExamples)
                {
                    group.Examples.Add($"`{Shorten(value.Value)}` in {sample}, line {value.LineNumber}");
                }
            }
        }

        public string Build()
        {
            lock (_lock)
            {
                var text = new StringBuilder();

                WriteHead(text);
                WriteSummary(text);
                WriteMissing(text);
                WriteGaps(text);
                WriteUnusedEntries(text);
                WriteNotRenderedGroups(text);
                WriteSamples(text);
                WriteSampleDetails(text);

                return text.ToString();
            }
        }

        public void WriteTo(string path)
        {
            string? folder = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(path, Build(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        private void WriteHead(StringBuilder text)
        {
            text.AppendLine("# XML value coverage report");
            text.AppendLine();
            text.AppendLine("This report tells you which values of the XML input are not on the rendered PDF.");
            text.AppendLine("The test case `XmlSample_AllValuesAreVisible` makes it.");
            text.AppendLine();
            text.AppendLine($"- Made on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}");
            text.AppendLine($"- Samples directory: `{SamplesDirectory}`");
            text.AppendLine($"- CLI tool: `{CliExecutable}`");
            text.AppendLine($"- XML samples: {_samples.Count}");
            text.AppendLine();
            text.AppendLine("The section [Each sample in detail](#each-sample-in-detail) names, for each");
            text.AppendLine("sample, the elements that are not on the page.");
            text.AppendLine();
        }

        private void WriteSummary(StringBuilder text)
        {
            int found = _samples.Sum(sample => sample.Found);
            int notRendered = _samples.Sum(sample => sample.NotRendered);
            int knownGap = _samples.Sum(sample => sample.KnownGap);
            int missing = _samples.Sum(sample => sample.Missing.Count);
            int all = found + notRendered + knownGap + missing;

            text.AppendLine("## Summary");
            text.AppendLine();
            text.AppendLine("| Result | Values | Part |");
            text.AppendLine("|---|---:|---:|");
            WriteSummaryRow(text, "Values in the XML input", all, all);
            WriteSummaryRow(text, "On the page", found, all);
            WriteSummaryRow(text, "Not rendered, by design", notRendered, all);
            WriteSummaryRow(text, "Known gap of the renderer", knownGap, all);
            WriteSummaryRow(text, "Missing, without an entry (test failure)", missing, all);
            text.AppendLine();
        }

        private static void WriteSummaryRow(StringBuilder text, string name, int count, int all)
        {
            string part = all == 0 ? "-" : $"{100.0 * count / all:F1} %";
            text.AppendLine($"| {name} | {count} | {part} |");
        }

        private void WriteMissing(StringBuilder text)
        {
            var withMissing = _samples.Where(sample => sample.Missing.Count > 0).ToList();

            text.AppendLine("## Values that are missing, and that have no entry");
            text.AppendLine();

            if (withMissing.Count == 0)
            {
                text.AppendLine("None. Each value is on the page, or an entry in `XmlValueExclusions.All` applies to it.");
                text.AppendLine();
                return;
            }

            text.AppendLine("These values make the test fail. Show the value on the page, or add an entry to");
            text.AppendLine("`XmlValueExclusions.All`.");
            text.AppendLine();

            foreach (SampleResult sample in withMissing)
            {
                text.AppendLine($"### {sample.Name}");
                text.AppendLine();
                text.AppendLine("| Line | Path | Value |");
                text.AppendLine("|---:|---|---|");

                foreach (XmlValue value in sample.Missing)
                {
                    text.AppendLine($"| {value.LineNumber} | `{value.Path}` | `{Shorten(value.Value)}` |");
                }

                text.AppendLine();
            }
        }

        private void WriteGaps(StringBuilder text)
        {
            text.AppendLine("## Known gaps of the renderer");
            text.AppendLine();

            if (_gaps.Count == 0)
            {
                text.AppendLine("None. The renderer shows each value that a reader of the invoice needs.");
                text.AppendLine();
                return;
            }

            int total = _gaps.Values.Sum(group => group.Count);

            text.AppendLine($"The renderer does not show {total} value(s), in {_gaps.Count} element(s).");
            text.AppendLine("Each line is a task for the renderer. If the renderer starts to show a value,");
            text.AppendLine("remove the entry from `XmlValueExclusions.All`.");
            text.AppendLine();
            text.AppendLine("| Values | Samples | Element | Reason |");
            text.AppendLine("|---:|---:|---|---|");

            foreach (GapGroup group in _gaps.Values.OrderByDescending(item => item.Count))
            {
                text.AppendLine(
                    $"| {group.Count} | {group.Samples.Count} | `{group.Exclusion.Pattern}` | {group.Exclusion.Reason} |");
            }

            text.AppendLine();
            text.AppendLine("### Examples");
            text.AppendLine();

            foreach (GapGroup group in _gaps.Values.OrderByDescending(item => item.Count))
            {
                text.AppendLine($"**`{group.Exclusion.Pattern}`** — {group.Exclusion.Reason}");
                text.AppendLine();

                foreach (string example in group.Examples)
                {
                    text.AppendLine($"- {example}");
                }

                if (group.Count > group.Examples.Count)
                {
                    text.AppendLine($"- ... and {group.Count - group.Examples.Count} more");
                }

                text.AppendLine();
            }
        }

        private void WriteUnusedEntries(StringBuilder text)
        {
            var unused = XmlValueExclusions.All
                .Where(exclusion => exclusion.Kind == ExclusionKind.KnownGap)
                .Where(exclusion => !_gaps.ContainsKey(exclusion.Pattern))
                .ToList();

            if (unused.Count == 0)
            {
                return;
            }

            text.AppendLine("## Entries of kind KnownGap without a hit");
            text.AppendLine();
            text.AppendLine("The samples give no value for these entries. You can remove them from");
            text.AppendLine("`XmlValueExclusions.All`.");
            text.AppendLine();

            foreach (XmlValueExclusion exclusion in unused)
            {
                text.AppendLine($"- `{exclusion.Pattern}` — {exclusion.Reason}");
            }

            text.AppendLine();
        }

        /*
         * The elements that the page does not show by design, over all samples.
         *
         * The section for the known gaps names the values that a reader misses. This
         * section names the values that the page must not show in the form of the XML,
         * for example a code.
         */
        private void WriteNotRenderedGroups(StringBuilder text)
        {
            var groups = _samples
                .SelectMany(sample => sample.NotOnPage
                    .Where(entry => entry.Kind == NotOnPageKind.NotRendered)
                    .Select(entry => new { sample.Name, Entry = entry }))
                .GroupBy(item => item.Entry.Pattern ?? string.Empty)
                .Select(group => new
                {
                    Pattern = group.Key,
                    Count = group.Count(),
                    Samples = group.Select(item => item.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
                    Reason = group.First().Entry.Reason ?? string.Empty
                })
                .OrderByDescending(group => group.Count)
                .ToList();

            text.AppendLine("## Elements that the page does not show, by design");
            text.AppendLine();

            if (groups.Count == 0)
            {
                text.AppendLine("None.");
                text.AppendLine();
                return;
            }

            text.AppendLine($"The page does not show {groups.Sum(group => group.Count)} value(s), " +
                            $"in {groups.Count} entry/entries. These values are codes and technical data.");
            text.AppendLine("The page shows their meaning in an other form, or it does not need them.");
            text.AppendLine();
            text.AppendLine("| Values | Samples | Entry | Reason |");
            text.AppendLine("|---:|---:|---|---|");

            foreach (var group in groups)
            {
                text.AppendLine($"| {group.Count} | {group.Samples} | `{group.Pattern}` | {group.Reason} |");
            }

            text.AppendLine();
        }

        /*
         * For each sample, the elements that are not on the page.
         *
         * The list holds one row for each element, with the number of values and the
         * kind. The kind tells if the page must not show the value, if the renderer has
         * a gap, or if no entry explains the value.
         */
        private void WriteSampleDetails(StringBuilder text)
        {
            text.AppendLine("## Each sample in detail");
            text.AppendLine();
            text.AppendLine("The elements of a sample that are not on the page. A sample without a row");
            text.AppendLine("shows each of its values.");
            text.AppendLine();

            foreach (SampleResult sample in _samples.OrderBy(item => item.Name, StringComparer.OrdinalIgnoreCase))
            {
                text.AppendLine($"### {sample.Name}");
                text.AppendLine();

                if (sample.NotOnPage.Count == 0)
                {
                    text.AppendLine("Each value of this sample is on the page.");
                    text.AppendLine();
                    continue;
                }

                text.AppendLine($"{sample.Found} of {sample.Found + sample.NotOnPage.Count} values are on the page.");
                text.AppendLine();
                text.AppendLine("| Values | Kind | Element |");
                text.AppendLine("|---:|---|---|");

                var rows = sample.NotOnPage
                    .GroupBy(entry => new { entry.Value.Path, entry.Kind })
                    .Select(group => new { group.Key.Path, group.Key.Kind, Count = group.Count() })
                    .OrderBy(row => row.Kind)
                    .ThenByDescending(row => row.Count)
                    .ThenBy(row => row.Path, StringComparer.Ordinal);

                foreach (var row in rows)
                {
                    string kind = row.Kind switch
                    {
                        NotOnPageKind.NotRendered => "not rendered",
                        NotOnPageKind.KnownGap => "known gap",
                        _ => "**missing**"
                    };

                    text.AppendLine($"| {row.Count} | {kind} | `{row.Path}` |");
                }

                text.AppendLine();
            }
        }

        private void WriteSamples(StringBuilder text)
        {
            text.AppendLine("## Each sample");
            text.AppendLine();
            text.AppendLine("| Sample | Values | On the page | Not rendered | Known gap | Missing |");
            text.AppendLine("|---|---:|---:|---:|---:|---:|");

            foreach (SampleResult sample in _samples.OrderBy(item => item.Name, StringComparer.OrdinalIgnoreCase))
            {
                int all = sample.Found + sample.NotRendered + sample.KnownGap + sample.Missing.Count;
                text.AppendLine(
                    $"| {sample.Name} | {all} | {sample.Found} | {sample.NotRendered} | " +
                    $"{sample.KnownGap} | {sample.Missing.Count} |");
            }

            text.AppendLine();
        }

        private static string Shorten(string value)
        {
            string plain = PdfTextContent.Normalize(value).Replace("|", "\\|");
            return plain.Length <= 70 ? plain : plain[..70] + "...";
        }

        private sealed class SampleResult
        {
            public required string Name { get; init; }
            public required int Found { get; init; }

            /* Each value of this sample that is not on the page. */
            public required List<NotOnPageValue> NotOnPage { get; init; }

            public int NotRendered => Count(NotOnPageKind.NotRendered);
            public int KnownGap => Count(NotOnPageKind.KnownGap);

            public List<XmlValue> Missing => NotOnPage
                .Where(entry => entry.Kind == NotOnPageKind.Missing)
                .Select(entry => entry.Value)
                .ToList();

            private int Count(NotOnPageKind kind) => NotOnPage.Count(entry => entry.Kind == kind);
        }

        private sealed class GapGroup
        {
            public required XmlValueExclusion Exclusion { get; init; }
            public int Count { get; set; }
            public HashSet<string> Samples { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public List<string> Examples { get; } = new List<string>();
        }
    }
}
