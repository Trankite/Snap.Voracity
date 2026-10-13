namespace Common.Source.Factory.Streams.Text.Interface
{
    public interface ITextWriter : IDisposable
    {
        void Write(char value);

        void Write(ReadOnlySpan<char> buffer);

        void WriteLine();

        void WriteLine(char value);

        void WriteLine(ReadOnlySpan<char> buffer);
    }
}