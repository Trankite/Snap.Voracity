using Common.Source.Service.Gradual.Abstract;

namespace Common.Source.Service.Streams.Open
{
    public class FileOpenRead : FileOpenStream
    {
        public override FileMode FileMode => FileMode.Open;

        public override FileAccess FileAccess => FileAccess.Read;

        public override FileShare FileShare => FileShare.None;

        public FileOpenRead(string filePath, bool leaveOpen = default) : base(filePath, default, leaveOpen) { }

        public static FileOpenRead Create(string filePath, bool leaveOpen = default)
        {
            return new FileOpenRead(filePath, leaveOpen).StartOrRetry();
        }
    }
}