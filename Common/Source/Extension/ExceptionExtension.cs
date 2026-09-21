using Common.Source.Core.Interface;
using Common.Source.Resource.Localization;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetMessage(this IExceptionCapturer exceptionCapturer)
        {
            return GetMessage(exceptionCapturer.CapturedException);
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
        public static Exception GetException(this IExceptionCapturer exceptionCapturer)
        {
            return GetException(exceptionCapturer.CapturedException);
        }

        [DebuggerStepThrough]
        public static Exception GetException(this ExceptionDispatchInfo? exceptionDispatchInfo)
        {
            return exceptionDispatchInfo?.SourceException ?? new NullReferenceException();
        }

        [DebuggerStepThrough]
        public static T ThrowIfExceptionCaptured<T>(this T exceptionCapturer) where T : IExceptionCapturer
        {
            exceptionCapturer.CapturedException?.Throw();
            return exceptionCapturer;
        }

        [DebuggerStepThrough]
        public static ExceptionDispatchInfo DispatchCapture(this IExceptionCapturer exceptionCapturer, Exception exception)
        {
            return exceptionCapturer.CapturedException = ExceptionDispatchInfo.Capture(exception);
        }
    }
}