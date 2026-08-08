namespace Pacioli.CLI.NET8.Test.Content
{
    /* One value of the XML input: the text of an element, or the value of an attribute. */
    public sealed class XmlValue
    {
        public required string Path { get; init; }
        public required string Name { get; init; }
        public required string Value { get; init; }
        public required bool IsAttribute { get; init; }
        public int LineNumber { get; init; }

        public override string ToString() => $"{Path} = \"{Value}\"";
    }
}
