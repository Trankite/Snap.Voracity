using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class HttpMessageExtension
    {
        [DebuggerStepThrough]
        public static bool IsSuccessStatusCodeOrThrow(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}