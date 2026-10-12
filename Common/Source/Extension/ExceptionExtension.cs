using Common.Source.Core.Interface;
using Common.Source.Resource.Localization;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetMessage(this IExceptionCaptureOwner exceptionCaptureOwner, object? message = default)
        {
            return GetMessage(exceptionCaptureOwner.CapturedException, message);
        }

        [DebuggerStepThrough]
        public static string GetMessage(this ExceptionDispatchInfo? exceptionDispatchInfo, object? message = default)
        {
            return GetMessage(exceptionDispatchInfo?.SourceException, message);
        }

        [DebuggerStepThrough]
        public static string GetMessage(this Exception? exception, object? message = default)
        {
            return LocalString.ExtensionExceptionMessageDescription.SafeFormat(exception?.Message ?? message);
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