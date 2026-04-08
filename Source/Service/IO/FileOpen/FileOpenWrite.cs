using StarRailDamage.Source.Extension;
using System.IO;

namespace StarRailDamage.Source.Service.IO.FileOpen
{
    public class FileOpenWrite : FileOpenStream
    {
        public FileOpenWrite(string path)
        {
            Success = StreamExtension.TryOpenWrite(path, out FileStream? FileStream, this) && true.Configure(Stream = FileStream);
        }

        public static FileOpenWrite Create(string path)
        {
            return new FileOpenWrite(FileHelper.BuildFilePath(path));
        }

        public FileOpenWrite(Stream stream) : base(stream) { }
    }
}