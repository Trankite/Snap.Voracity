using System.IO;

namespace StarRailDamage.Source.Service.FileOpen
{
    public class FileOpenRead : FileOpenStream
    {
        public FileOpenRead(string path) : base(path, FileMode.Open, FileAccess.Read) { }
    }
}