using StarRailDamage.Source.Extension;
using System.IO;

namespace StarRailDamage.Source.Service.IO.FileOpen
{
    public class FileOpenRead : FileOpenStream
    {
        public FileOpenRead(string path)
        {
            Success = StreamExtension.TryOpenRead(path, out FileStream? FileStream, this) && true.Configure(Stream = FileStream);
        }

        public FileOpenRead(Stream stream) : base(stream) { }
    }
}