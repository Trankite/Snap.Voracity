using Common.Source.Extension;
using Common.Source.Resource.Localization;
using System.Collections.Frozen;

namespace Common.Source.Factory.Streams.FileSave.Metadata
{
    public static class FileFormatExtension
    {
        private static readonly FrozenDictionary<string, FileFormat> FormatCache;

        public static bool TryParse(this string format, out FileFormat value)
        {
            return FormatCache.TryGetValue(format, out value);
        }

        public static FileFormat Parse(this string format, FileFormat defaultValue = default)
        {
            return FormatCache.TryGetValue(format, out FileFormat FileFormat) ? FileFormat : defaultValue;
        }

        public static string UnSupported(this FileFormat format)
        {
            return LocalString.CoreMetadataExceptionUnSupportedFormat.SafeFormat(format.ToString());
        }

        public static string ChangeExtension(this FileFormat format, string filePath)
        {
            return Path.ChangeExtension(filePath, format.GetDescription());
        }

        static FileFormatExtension()
        {
            FormatCache = Enum.GetValues<FileFormat>().ToFrozenDictionary(EnumExtension.GetDescription);
        }
    }
}