using Common.Source.Factory.Streams.FileSave.Metadata;

namespace Common.Source.Factory.Streams.FileSave.Interface
{
    public interface IFileFormatSaver
    {
        bool IsSupported(FileFormat format);

        void SaveToFormat(Stream stream, FileFormat format);
    }
}