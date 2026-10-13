namespace Common.Source.Factory.Streams.Text.Interface
{
    public interface ITextStream : IDisposable
    {
        ITextReader Reader { get; }

        ITextWriter Writer { get; }
    }
}