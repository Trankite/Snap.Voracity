using System.IO;

namespace StarRailDamage.Source.Service.FileOpen
{
    public class FileOpenWrite : FileOpenStream
    {
        public FileOpenWrite(string path, bool create = false) : base(path, FileMode.Create, FileAccess.Write, FileShare.None, create) { }

        public static FileOpenWrite Create(string path) => new(path, true);
    }
}