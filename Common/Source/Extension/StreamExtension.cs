using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class StreamExtension
    {
        [DebuggerStepThrough]
        public static long GetLength(this Stream stream)
        {
            if (stream.CanSeek)
            {
                try { return stream.Length; } catch { }
            }
            return -1;
        }
    }
}