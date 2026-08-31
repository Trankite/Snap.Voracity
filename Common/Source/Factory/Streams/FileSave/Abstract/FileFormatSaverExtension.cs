using Common.Source.Factory.Streams.FileSave.Interface;
using Common.Source.Factory.Streams.FileSave.Metadata;

namespace Common.Source.Factory.Streams.FileSave.Abstract
{
    public static class FileFormatSaverExtension
    {
        public static bool TrySaveToFormat(this IFileFormatSaver saver, Stream stream, FileFormat format)
        {
            if (saver.IsSupported(format))
            {
                saver.SaveToFormat(stream, format);
                return true;
            }
            return false;
        }
    }
}