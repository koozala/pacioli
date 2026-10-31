namespace Pacioli.CLI.Test.Content
{
    /* Tells why the test does not look for a value of the XML input. */
    public enum ExclusionKind
    {
        /*
         * The renderer does not write the value in this form, and it must not.
         * A code is an example: the XML holds "H87", and the page shows "Anz.".
         */
        NotRendered,

        /*
         * The renderer does not show the value, but a reader of the invoice needs it.
         * Each entry of this kind is a task for the renderer. If the renderer starts to
         * show the value, remove the entry.
         */
        KnownGap
    }

    /*
     * One entry that removes a value of the XML input from the check.
     *
     * The pattern has four forms:
     *
     *   "@name"      The attribute with this name.
     *   "/a/b/"      Each value whose path holds this part.
     *   "/a/b"       Each value whose path ends with this part.
     *   "name"       The element with this name.
     *
     * An entry can also give a condition on the value. Then the entry applies only to a
     * value that agrees with the condition.
     */
    public sealed class XmlValueExclusion
    {
        public XmlValueExclusion(string pattern, ExclusionKind kind, string reason,
            Func<string, bool>? valueCondition = null)
        {
            Pattern = pattern;
            Kind = kind;
            Reason = reason;
            ValueCondition = valueCondition;
        }

        public string Pattern { get; }
        public ExclusionKind Kind { get; }
        public string Reason { get; }
        public Func<string, bool>? ValueCondition { get; }

        public bool Matches(XmlValue value)
        {
            if (ValueCondition != null && !ValueCondition(value.Value))
            {
                return false;
            }

            if (Pattern.StartsWith('@'))
            {
                return value.IsAttribute &&
                       string.Equals(value.Name, Pattern[1..], StringComparison.Ordinal);
            }

            if (Pattern.StartsWith('/'))
            {
                return Pattern.EndsWith('/')
                    ? value.Path.Contains(Pattern, StringComparison.Ordinal)
                    : value.Path.EndsWith(Pattern, StringComparison.Ordinal);
            }

            return !value.IsAttribute &&
                   string.Equals(value.Name, Pattern, StringComparison.Ordinal);
        }
    }
}
