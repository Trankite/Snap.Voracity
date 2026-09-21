using Common.Source.Extension;
using Common.Source.Service.Mission.Gradual.Abstract;

namespace Common.Source.Factory.Streams.FileOpen
{
    public class FileOpenRead : FileOpenStream
    {
        public override FileMode FileMode => FileMode.Open;

        public override FileAccess FileAccess => FileAccess.Read;

        public override FileShare FileShare => FileShare.None;

        public FileOpenRead(string filePath, bool leaveOpen = default) : base(filePath, default, leaveOpen) { }

        public static FileOpenRead Create(string filePath, bool leaveOpen = default)
        {
            return new FileOpenRead(filePath, leaveOpen).Configure(Self => Self.StartOrRetry());
        }
    }
}