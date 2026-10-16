using System.Diagnostics;
using System.Text;

namespace Common.Source.Extension
{
    public static class StringBuilderExtension
    {
        [DebuggerStepThrough]
        public static bool StartsWith(this StringBuilder builder, char value)
        {
            return builder.Length > 0 && builder[0] == value;
        }

        [DebuggerStepThrough]
        public static bool StartsWith(this StringBuilder builder, ReadOnlySpan<char> values)
        {
            if (builder.Length < values.Length)
            {
                return false;
            }
            for (int i = 0; i < values.Length; i++)
            {
                if (builder[i] != values[i]) return false;
            }
            return true;
        }

        [DebuggerStepThrough]
        public static bool EndsWith(this StringBuilder builder, char value)
        {
            return builder.Length > 0 && builder[^1] == value;
        }

        [DebuggerStepThrough]
        public static bool EndsWith(this StringBuilder builder, ReadOnlySpan<char> values)
        {
            if (builder.Length < values.Length)
            {
                return false;
            }
            for (int i = 0; i < values.Length; i++)
            {
                if (builder[^(i + 1)] != values[i]) return false;
            }
            return true;
        }

        [DebuggerStepThrough]
        public static string Complete(this StringBuilder builder)
        {
            return builder.ToString().Configure(builder.Clear());
        }

        [DebuggerStepThrough]
        public static string Complete(this StringBuilder builder, ReadOnlySpan<char> span)
        {
            return builder.Length > 0 ? Complete(builder.Append(span)) : span.ToString();
        }

        [DebuggerStepThrough]
        public static string Discard(this StringBuilder builder)
        {
            return builder.Clear().Captured(string.Empty);
        }
    }
}