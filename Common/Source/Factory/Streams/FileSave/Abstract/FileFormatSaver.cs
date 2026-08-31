using Common.Source.Factory.Streams.FileSave.Interface;
using Common.Source.Factory.Streams.FileSave.Metadata;
using System.Collections.Frozen;

namespace Common.Source.Factory.Streams.FileSave.Abstract
{
    public abstract class FileFormatSaver<T> : IFileFormatSaver where T : FileFormatSaver<T>
    {
        protected abstract T Sender { get; }

        protected abstract FrozenDictionary<FileFormat, Action<T, Stream>> FileSaveActionTable { get; }

        public bool IsSupported(FileFormat format)
        {
            return FileSaveActionTable.ContainsKey(format);
        }

        public void SaveToFormat(Stream stream, FileFormat format)
        {
            FileSaveActionTable.GetValueOrDefault(format)?.Invoke(Sender, stream);
        }
    }
}