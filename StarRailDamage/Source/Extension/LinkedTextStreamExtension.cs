using StarRailDamage.Source.Core.Abstraction;

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

        public static void WriteLine(this ILinkedTextStream command, object value)
        {
            command.WriteLine(value.ToString());
        }

        public static void WriteLine(this ILinkedTextStream stream, ReadOnlySpan<char> value)
        {
            if (value.Length > 0)
            {
                stream.Writer.WriteLine(value);
            }
        }

        public static int Read(this ILinkedTextStream stream)
        {
            return stream.Reader.Read();
        }

        public static string ReadLine(this ILinkedTextStream stream)
        {
            return stream.Reader.ReadLine().NotNull();
        }

        public static string ReadLine(this ILinkedTextStream stream, ReadOnlySpan<char> value)
        {
            stream.Writer.Write(value);
            return stream.ReadLine();
        }
    }
}