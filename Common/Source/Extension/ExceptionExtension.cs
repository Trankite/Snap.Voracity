using Common.Source.Resource.Localization;
using Common.Source.Service.Mission.Interface;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetExceptionMessage(this IExceptionCapturer exceptionCapturer, object? message = default)
        {
            return GetExceptionMessage(exceptionCapturer.CapturedException, message);
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
        public static void ThrowIfExceptionCaptured(this IExceptionCapturer exceptionCapturer)
        {
            exceptionCapturer.CapturedException?.Throw();
        }

        [StackTraceHidden]
        [DebuggerStepThrough]
        public static ExceptionDispatchInfo DispatchCapture(this Exception exception, bool setStackTrace = default)
        {
            return ExceptionDispatchInfo.Capture(setStackTrace ? ExceptionDispatchInfo.SetCurrentStackTrace(exception) : exception);
        }
    }
}