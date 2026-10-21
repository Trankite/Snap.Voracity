using Common.Source.Core.Interface;
using Common.Source.Resource.Localization;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetExceptionMessage(this IExceptionCaptureOwner exceptionCaptureOwner, object? message = default)
        {
            return GetExceptionMessage(exceptionCaptureOwner.CapturedException, message);
        }

        [DebuggerStepThrough]
        public static string GetExceptionMessage(this ExceptionDispatchInfo? exceptionDispatchInfo, object? message = default)
        {
            return GetExceptionMessage(exceptionDispatchInfo?.SourceException, message);
        }

        [DebuggerStepThrough]
        public static string GetExceptionMessage(this Exception? exception, object? message = default)
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

        [StackTraceHidden]
        [DebuggerStepThrough]
        public static ExceptionDispatchInfo DispatchCapture(this Exception exception, bool setStackTrace = default)
        {
            return ExceptionDispatchInfo.Capture(setStackTrace ? ExceptionDispatchInfo.SetCurrentStackTrace(exception) : exception);
        }
    }
}