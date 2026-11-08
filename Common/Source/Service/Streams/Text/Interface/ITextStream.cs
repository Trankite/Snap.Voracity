namespace Common.Source.Service.Streams.Text.Interface
{
    public interface ITextStream : IDisposable
    {
        ITextReader Reader { get; }

        ITextWriter Writer { get; }
    }
}