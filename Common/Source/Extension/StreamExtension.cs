namespace Common.Source.Extension
{
    public static class StreamExtension
    {
        public static bool TryRead(this Stream stream, Span<byte> buffer, out int count)
        {
            return (count = stream.Read(buffer)) > 0;
        }

        public static bool TryRead(this Stream stream, byte[] buffer, int index, int length, out int count)
        {
            return (count = stream.Read(buffer, index, length)) > 0;
        }

        public static bool TryRead(this TextReader reader, Span<char> buffer, out int count)
        {
            return (count = reader.Read(buffer)) > 0;
        }

        public static bool TryRead(this TextReader reader, char[] buffer, int index, int length, out int count)
        {
            return (count = reader.Read(buffer, index, length)) > 0;
        }
    }
}