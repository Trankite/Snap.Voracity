namespace Common.Source.Service.Terminal.Abstraction
{
    public interface ILinkedTextStream
    {
        TextWriter Writer { get; set; }

        TextReader Reader { get; set; }
    }
}