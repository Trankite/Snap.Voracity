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
            if (value.Length > 0) stream.Writer.WriteLine(value);
        }

        public static bool Enquire(this ILinkedTextStream stream, string message)
        {
            ShowEnquireMessage(stream, message);
            return GetEnquireState(stream.ReadLine());
        }

        public static async ValueTask<bool> EnquireAsync(this ILinkedTextStream stream, string message, CancellationToken cancellationToken = default)
        {
            ShowEnquireMessage(stream, message);
            return GetEnquireState(await stream.ReadLineAsync(cancellationToken));
        }

        private static void ShowEnquireMessage(ILinkedTextStream stream, string message)
        {
            stream.WriteLine($"{message} (Y/N)");
        }

        private static bool GetEnquireState(string value)
        {
            return !value.StartsWith("N", StringComparison.OrdinalIgnoreCase);
        }

        public static int Read(this ILinkedTextStream stream)
        {
            return stream.Reader.Read();
        }

        public static string ReadLine(this ILinkedTextStream stream)
        {
            try { return stream.Reader.ReadLine().NotNull(); } catch { return string.Empty; }
        }

        public static async ValueTask<string> ReadLineAsync(this ILinkedTextStream stream, CancellationToken cancellationToken = default)
        {
            try { return await stream.Reader.ReadLineAsync(cancellationToken) ?? string.Empty; } catch { return string.Empty; }
        }

        public static string ReadLine(this ILinkedTextStream stream, ReadOnlySpan<char> value)
        {
            stream.Write(value);
            return stream.ReadLine();
        }

        public static ValueTask<string> ReadLineAsync(this ILinkedTextStream stream, string value, CancellationToken cancellationToken = default)
        {
            stream.Write(value);
            return stream.ReadLineAsync(cancellationToken);
        }
    }
}