namespace Pacioli.CLI.NET8.Test.Content
{
    /* Tells why a value of the XML input is not on the rendered page. */
    public enum NotOnPageKind
    {
        /*
         * The renderer must not write the value in this form. A code is an example: the
         * XML holds "H87", and the page shows "Anz.".
         */
        NotRendered,

        /* The renderer does not show the value, but a reader of the invoice needs it. */
        KnownGap,

        /* No entry explains the value. This makes the test fail. */
        Missing
    }

    /* One value of the XML input that is not on the rendered page. */
    public sealed class NotOnPageValue
    {
        public required XmlValue Value { get; init; }

        public required NotOnPageKind Kind { get; init; }

        /* The pattern of the entry that applies, or null for a missing value. */
        public string? Pattern { get; init; }

        /* The reason of the entry that applies, or null for a missing value. */
        public string? Reason { get; init; }

        public string KindText => Kind switch
        {
            NotOnPageKind.NotRendered => "not rendered",
            NotOnPageKind.KnownGap => "known gap",
            _ => "missing"
        };
    }
}
