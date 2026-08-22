using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Extension
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

        public static string ReadLine(this ILinkedTextStream stream)
        {
            try
            {
                return stream.Reader.ReadLine().NotNull();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async ValueTask<string> ReadLineAsync(this ILinkedTextStream stream, CancellationToken cancellationToken = default)
        {
            try
            {
                return (await stream.Reader.ReadLineAsync(cancellationToken)).NotNull();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ReadLine(this ILinkedTextStream stream, ReadOnlySpan<char> value)
        {
            stream.Writer.Write(value);
            return stream.ReadLine();
        }

        public static ValueTask<string> ReadLineAsync(this ILinkedTextStream stream, string value, CancellationToken cancellationToken = default)
        {
            stream.Writer.Write(value);
            return stream.ReadLineAsync(cancellationToken);
        }
    }
}