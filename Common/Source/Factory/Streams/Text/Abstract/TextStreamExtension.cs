using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public static class TextStreamExtension
    {
        public static void Write(this ITextStream stream, ReadOnlySpan<char> value)
        {
            stream.Writer.Write(value);
        }

        public static void WriteLine(this ITextStream stream)
        {
            stream.Writer.WriteLine();
        }

        public static void WriteLine(this ITextStream stream, object value)
        {
            stream.WriteLine(value.ToString());
        }

        public static void WriteLine(this ITextStream stream, ReadOnlySpan<char> value)
        {
            if (value.Length > 0)
            {
                stream.Writer.WriteLine(value);
            }
        }

        public static bool Enquire(this ITextStream stream, string message)
        {
            ShowEnquireMessage(stream, message);
            return GetEnquireState(stream.ReadLine());
        }

        public static async ValueTask<bool> EnquireAsync(this ITextStream stream, string message, CancellationToken cancellationToken = default)
        {
            ShowEnquireMessage(stream, message);
            return GetEnquireState(await stream.ReadLineAsync(cancellationToken).ConfigureAwait(false));
        }

        private static void ShowEnquireMessage(ITextStream stream, string message)
        {
            stream.WriteLine($"{message}\x20(Y/N)");
        }

        private static bool GetEnquireState(string? value)
        {
            return value.IsNotNull() && !value.StartsWith("N", StringComparison.OrdinalIgnoreCase);
        }

        public static int Read(this ITextStream stream)
        {
            return stream.Reader.Read();
        }

        public static string? ReadLine(this ITextStream stream)
        {
            try { return stream.Reader.ReadLine(); } catch { return string.Empty; }
        }

        public static async ValueTask<string?> ReadLineAsync(this ITextStream stream, CancellationToken cancellationToken = default)
        {
            try { return await stream.Reader.ReadLineAsync(cancellationToken).ConfigureAwait(false); } catch { return string.Empty; }
        }

        public static ValueTask<string?> ReadLineAsync(this ITextStream stream, string value, CancellationToken cancellationToken = default)
        {
            stream.Write(value);
            return stream.ReadLineAsync(cancellationToken);
        }
    }
}