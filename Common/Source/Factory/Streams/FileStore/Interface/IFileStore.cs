using Common.Source.Factory.Streams.FileStore.Metadata;

namespace Common.Source.Factory.Streams.FileStore.Interface
{
    public interface IFileStore
    {
        bool IsSupported(FileFormat format);

        void SaveToFormat(Stream stream, FileFormat format);
    }
}