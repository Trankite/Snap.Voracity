namespace SourceBuilder.Source
{
    public sealed class CodeBuilder : IDisposable
    {
        private readonly StreamWriter Stream;

        private static readonly string FilePath;

        public CodeBuilder()
        {
            Stream = new StreamWriter(FilePath);
        }

        public void Write(string? value) => Stream.Write(value);

        public void WriteLine(string? value = null) => Stream.WriteLine(value);

        public void Dispose()
        {
            Stream?.Dispose();
        }

        static CodeBuilder()
        {
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "BuildCode.cs");
        }
    }
}