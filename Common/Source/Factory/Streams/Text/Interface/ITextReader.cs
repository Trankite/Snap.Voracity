namespace Common.Source.Factory.Streams.Text.Interface
{
    public interface ITextReader : IDisposable
    {
        int Read();

        ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default);

        string? ReadLine();

        ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken = default);
    }
}