using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class RandomExtension
    {
        [DebuggerStepThrough]
        public static string GetLowerHexString(this Random random, int length)
        {
            Span<char> buffer = stackalloc char[length];
            random.GetItems("0123456789abcdef", buffer);
            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static string GetLowerAndNumberString(this Random random, int length)
        {
            Span<char> buffer = stackalloc char[length];
            random.GetItems("0123456789abcdefghijklmnopqrstuvwxyz", buffer);
            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static string GetUpperAndNumberString(this Random random, int length)
        {
            Span<char> buffer = stackalloc char[length];
            random.GetItems("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", buffer);
            return buffer.ToString();
        }
    }
}