namespace Common.Source.Factory.Streams.FileOpen
{
    public class FileOpenWrite : FileOpenStream
    {
        public FileOpenWrite(string path, bool create = default, bool leaveOpen = default) : base(path, FileMode.Create, FileAccess.Write, FileShare.None, create, leaveOpen) { }

        public static FileOpenWrite Create(string path, bool leaveOpen = default) => new(path, true, leaveOpen);
    }
}