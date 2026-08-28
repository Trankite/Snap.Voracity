using System.Text;

namespace Common.Source.Extension
{
    public static class StringBuilderExtension
    {
        public static bool StartsWith(this StringBuilder builder, char value)
        {
            return builder.Length > 0 && builder[0] == value;
        }

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

        public static bool EndsWith(this StringBuilder builder, char value)
        {
            return builder.Length > 0 && builder[^1] == value;
        }

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

        public static string Complete(this StringBuilder builder)
        {
            return builder.ToString().Configure(builder.Clear());
        }
    }
}