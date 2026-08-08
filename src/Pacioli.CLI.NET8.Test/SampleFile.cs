namespace Pacioli.CLI.NET8.Test
{
    /* Describes one sample document. */
    public sealed class SampleFile
    {
        public SampleFile(int index, string fullPath, string relativePath)
        {
            Index = index;
            FullPath = fullPath;
            RelativePath = relativePath;
        }

        /* A number that starts at 1. The test uses it to make a unique output directory. */
        public int Index { get; }

        public string FullPath { get; }

        /* The path below the samples directory. The test report shows this text. */
        public string RelativePath { get; }

        public override string ToString() => RelativePath;
    }
}
