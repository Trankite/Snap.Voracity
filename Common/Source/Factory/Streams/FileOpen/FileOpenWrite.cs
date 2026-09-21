using Common.Source.Extension;
using Common.Source.Service.Mission.Gradual.Abstract;

namespace Common.Source.Factory.Streams.FileOpen
{
    public class FileOpenWrite : FileOpenStream
    {
        public override FileMode FileMode => FileMode.Create;

        public override FileAccess FileAccess => FileAccess.Write;

        public override FileShare FileShare => FileShare.None;

        public FileOpenWrite(string filePath, bool buildDirectory = default, bool leaveOpen = default) : base(filePath, buildDirectory, leaveOpen) { }

        public static FileOpenWrite Create(string filePath, bool buildDirectory = default, bool leaveOpen = default)
        {
            return new FileOpenWrite(filePath, buildDirectory, leaveOpen).Configure(Self => Self.StartOrRetry());
        }
    }
}