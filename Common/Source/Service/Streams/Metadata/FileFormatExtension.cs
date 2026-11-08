using Common.Source.Extension;
using Common.Source.Resource.Localization;
using System.Collections.Frozen;
using System.Diagnostics;

namespace Common.Source.Service.Streams.Metadata
{
    public static class FileFormatExtension
    {
        private static readonly FrozenDictionary<string, FileFormat> FormatTable;

        public static bool TryParse(string format, out FileFormat result)
        {
            return FormatTable.TryGetValue(format, out result);
        }

        public static FileFormat Parse(string format, FileFormat defaultValue = default)
        {
            return FormatTable.TryGetValue(format, out FileFormat fileFormat) ? fileFormat : defaultValue;
        }

        public static string GetUnSupportedMessage(this FileFormat format)
        {
            return LocalString.ServiceStreamsFormatExceptionUnSupported.SafeFormat(format);
        }

        [DebuggerStepThrough]
        public static string ChangeExtension(this FileFormat format, string filePath)
        {
            return Path.ChangeExtension(filePath, format is FileFormat.None or FileFormat.Optimal ? default : format.GetDescription());
        }

        static FileFormatExtension()
        {
            FormatTable = Enum.GetValues<FileFormat>().ToFrozenDictionary(EnumExtension.GetDescription);
        }
    }
}