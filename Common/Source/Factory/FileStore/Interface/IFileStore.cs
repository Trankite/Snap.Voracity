using Common.Source.Service.Streams.Metadata;

namespace Common.Source.Factory.FileStore.Interface
{
    public interface IFileStore
    {
        bool IsSupported(FileFormat format);

        void SaveToFormat(Stream stream, FileFormat format);
    }
}