namespace Common.Source.Service.Streams.Text.Interface
{
    public interface ITextStreamOwner
    {
        ITextStream? TextStream { get; set; }
    }
}