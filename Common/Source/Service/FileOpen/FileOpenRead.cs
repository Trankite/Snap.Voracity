namespace Common.Source.Service.FileOpen
{
    public class FileOpenRead : FileOpenStream
    {
        public FileOpenRead(string path, bool leaveOpen = default) : base(path, FileMode.Open, FileAccess.Read, leaveOpen: leaveOpen) { }
    }
}