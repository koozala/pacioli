namespace Pacioli.CLI.NET8.Test.Content
{
    /* Tells what happened when the test looked for one element. */
    public enum ElementCheckOutcome
    {
        /* The element is on the page. */
        Found,

        /* The element is not on the page. This is a failure. */
        Missing,

        /* The invoice has no value for this element. The test does not check it. */
        NotApplicable,

        /* The rule itself failed. This is a failure. */
        Error
    }

    /* Holds the result of one element check. */
    public sealed class ElementCheckResult
    {
        public required string RuleName { get; init; }

        /* The value that the test looked for. The report shows this text. */
        public required string Value { get; init; }

        public required ElementCheckOutcome Outcome { get; init; }

        /* The forms of the value that the test accepts. A date has more than one form. */
        public IReadOnlyList<string> Candidates { get; init; } = Array.Empty<string>();

        /* The message of the exception, if the rule failed. */
        public string? Error { get; init; }

        public bool IsFailure => Outcome is ElementCheckOutcome.Missing or ElementCheckOutcome.Error;

        public string Describe()
        {
            if (Outcome == ElementCheckOutcome.Error)
            {
                return $"{RuleName}: the rule failed with the message \"{Error}\"";
            }

            string forms = Candidates.Count > 1
                ? $" (accepted forms: {string.Join(" | ", Candidates.Select(c => $"\"{c}\""))})"
                : string.Empty;

            return $"{RuleName}: \"{Value}\"{forms}";
        }
    }
}
