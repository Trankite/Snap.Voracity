namespace Common.Source.Service.Terminal.Abstract
{
    public interface ILinkedTextStream
    {
        TextWriter Writer { get; set; }

        TextReader Reader { get; set; }
    }
}