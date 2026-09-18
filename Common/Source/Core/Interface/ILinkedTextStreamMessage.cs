using Common.Source.Service.Terminal.Abstract;

namespace Common.Source.Core.Interface
{
    public interface ILinkedTextStreamMessage
    {
        ILinkedTextStream? LinkedStream { get; set; }
    }
}