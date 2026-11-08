using Common.Source.Model.Interface;
using Common.Source.Service.Helper;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetExceptionMessage(this IExceptionable exceptionable, object? message = default)
        {
            return GetExceptionMessage(exceptionable.CapturedException, message);
        }

        [DebuggerStepThrough]
        public static string GetExceptionMessage(this ExceptionDispatchInfo? exceptionDispatchInfo, object? message = default)
        {
            return GetExceptionMessage(exceptionDispatchInfo?.SourceException, message);
        }

        [DebuggerStepThrough]
        public static string GetExceptionMessage(this Exception? exception, object? message = default)
        {
            return ExceptionHelper.GetExceptionMessage(exception, message);
        }

        [DebuggerStepThrough]
        public static void ThrowIfExceptionCaptured(this IExceptionable exceptionable)
        {
            exceptionable.CapturedException?.Throw();
        }

        [StackTraceHidden]
        [DebuggerStepThrough]
        public static ExceptionDispatchInfo DispatchCapture(this Exception exception, bool setStackTrace = default)
        {
            return ExceptionDispatchInfo.Capture(setStackTrace ? ExceptionDispatchInfo.SetCurrentStackTrace(exception) : exception);
        }
    }
}