using Common.Source.Core.Interface;
using Common.Source.Resource.Localization;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Common.Source.Extension
{
    public static class ExceptionExtension
    {
        [DebuggerStepThrough]
        public static string GetMessage(this IExceptionCapture exceptionCapture)
        {
            return GetMessage(exceptionCapture.CapturedException);
        }

        [DebuggerStepThrough]
        public static string GetMessage(this ExceptionDispatchInfo? dispatchInfo)
        {
            return GetMessage(dispatchInfo?.SourceException);
        }

        [DebuggerStepThrough]
        public static string GetMessage(this Exception? exception)
        {
            return LocalString.ExtensionExceptionMessageDescription.SafeFormat(exception?.Message);
        }

        [DebuggerStepThrough]
        public static ExceptionDispatchInfo DispatchCapture(this IExceptionCapture exceptionCapture, Exception exception)
        {
            return exceptionCapture.CapturedException = ExceptionDispatchInfo.Capture(exception);
        }
    }
}