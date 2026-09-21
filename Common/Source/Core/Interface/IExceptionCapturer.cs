using System.Runtime.ExceptionServices;

namespace Common.Source.Core.Interface
{
    public interface IExceptionCapturer
    {
        ExceptionDispatchInfo? CapturedException { get; set; }
    }
}