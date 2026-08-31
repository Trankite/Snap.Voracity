using System.Text;

namespace Common.Source.Extension
{
    public static class EncodingExtension
    {
        public static Encoding NotNull(this Encoding? encoding)
        {
            return encoding ?? Encoding.UTF8;
        }
    }
}