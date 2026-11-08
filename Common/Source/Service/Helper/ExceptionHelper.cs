using Common.Source.Extension;
using Common.Source.Resource.Localization;
using System.Diagnostics;

namespace Common.Source.Service.Helper
{
    public static class ExceptionHelper
    {
        [DebuggerStepThrough]
        public static string GetExceptionMessage(Exception? exception, object? message = default)
        {
            return LocalString.ServiceHelperExceptionMessageDescription.SafeFormat(exception?.Message ?? message);
        }
    }
}