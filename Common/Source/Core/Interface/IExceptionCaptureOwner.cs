using System.Runtime.ExceptionServices;

namespace Common.Source.Core.Interface
{
    public interface IExceptionCaptureOwner
    {
        ExceptionDispatchInfo? CapturedException { get; set; }

        void ThrowIfFailed();
    }
}