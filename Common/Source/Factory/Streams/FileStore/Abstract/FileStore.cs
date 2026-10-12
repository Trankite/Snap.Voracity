using Common.Source.Factory.Streams.FileStore.Interface;
using Common.Source.Factory.Streams.FileStore.Metadata;
using System.Collections.Frozen;

namespace Common.Source.Factory.Streams.FileStore.Abstract
{
    public abstract class FileStore<T> : IFileStore where T : FileStore<T>
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