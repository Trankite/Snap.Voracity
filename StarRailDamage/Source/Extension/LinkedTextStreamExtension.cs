using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Extension
{
    public static class LinkedTextStreamExtension
    {
        public static void Write(this ILinkedTextStream stream, ReadOnlySpan<char> value)
        {
            stream.Writer.Write(value);
        }

        public static void WriteLine(this ILinkedTextStream stream)
        {
            stream.Writer.WriteLine();
        }

        public static void WriteLine(this ILinkedTextStream stream, object value)
        {
            stream.WriteLine(value.ToString());
        }

        public static void WriteLine(this ILinkedTextStream stream, ReadOnlySpan<char> value)
        {
            if (value.Length > 0)
            {
                stream.Writer.WriteLine(value);
            }
        }

        public static string ReadLine(this ILinkedTextStream stream, CancellationToken cancellationToken = default)
        {
            try
            {
                return stream.Reader.ReadLineAsync(cancellationToken).AsTask().Result.NotNull();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ReadLine(this ILinkedTextStream stream, ReadOnlySpan<char> value, CancellationToken cancellationToken = default)
        {
            stream.Writer.Write(value);
            return stream.ReadLine(cancellationToken);
        }
    }
}