using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class UriExtension
    {
        [DebuggerStepThrough]
        public static string GetExtension(this Uri uri)
        {
            return Path.GetExtension(uri.AbsolutePath);
        }
    }
}