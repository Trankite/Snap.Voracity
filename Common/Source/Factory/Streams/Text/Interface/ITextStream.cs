namespace Common.Source.Factory.Streams.Text.Interface
{
    public interface ITextStream
    {
        TextWriter Writer { get; set; }

        TextReader Reader { get; set; }
    }
}