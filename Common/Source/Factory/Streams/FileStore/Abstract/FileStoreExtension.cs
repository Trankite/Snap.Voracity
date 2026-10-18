using Common.Source.Extension;
using Common.Source.Factory.Streams.FileStore.Interface;
using Common.Source.Factory.Streams.FileStore.Metadata;
using System.Diagnostics;

namespace Common.Source.Factory.Streams.FileStore.Abstract
{
    public static class FileStoreExtension
    {
        [DebuggerStepThrough]
        public static bool TrySaveToFormat(this IFileStore saver, Stream stream, FileFormat format)
        {
            if (saver.IsSupported(format).OutSender(out bool IsSupported))
            {
                saver.SaveToFormat(stream, format);
            }
            return IsSupported;
        }
    }
}