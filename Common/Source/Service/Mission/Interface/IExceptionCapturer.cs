using System.Runtime.ExceptionServices;

namespace Common.Source.Service.Mission.Interface
{
    public interface IExceptionCapturer
    {
        ExceptionDispatchInfo? CapturedException { get; }
    }
}