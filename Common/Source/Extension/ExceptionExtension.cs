using Common.Source.Core.Interface;
using Common.Source.Resource.Localization;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetMessage(this IExceptionCaptureOwner exceptionCaptureOwner)
        {
            return GetMessage(exceptionCaptureOwner.CapturedException);
        }

        [DebuggerStepThrough]
        public static string GetMessage(this ExceptionDispatchInfo? exceptionDispatchInfo)
        {
            return GetMessage(exceptionDispatchInfo?.SourceException);
        }

        [DebuggerStepThrough]
        public static string GetMessage(this Exception? exception)
        {
            return LocalString.ExtensionExceptionMessageDescription.SafeFormat(exception?.Message);
        }

        [DebuggerStepThrough]
        public static Exception GetException(this IExceptionCaptureOwner exceptionCaptureOwner)
        {
            return GetException(exceptionCaptureOwner.CapturedException);
        }

        [DebuggerStepThrough]
        public static Exception GetException(this ExceptionDispatchInfo? exceptionDispatchInfo)
        {
            return exceptionDispatchInfo?.SourceException ?? new NullReferenceException();
        }

        [DebuggerStepThrough]
        public static T ThrowIfExceptionCaptured<T>(this T exceptionCaptureOwner) where T : IExceptionCaptureOwner
        {
            exceptionCaptureOwner.CapturedException?.Throw();
            return exceptionCaptureOwner;
        }

        [DebuggerStepThrough]
        public static ExceptionDispatchInfo DispatchCapture(this IExceptionCaptureOwner exceptionCaptureOwner, Exception exception)
        {
            return exceptionCaptureOwner.CapturedException = ExceptionDispatchInfo.Capture(exception);
        }
    }
}