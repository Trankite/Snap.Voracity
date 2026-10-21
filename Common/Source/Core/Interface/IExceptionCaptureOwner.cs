using System.Runtime.ExceptionServices;

namespace Common.Source.Core.Interface
{
    public interface IExceptionCaptureOwner
    {
        ExceptionDispatchInfo? CapturedException { get; }

        void ThrowIfFailed();
    }
}