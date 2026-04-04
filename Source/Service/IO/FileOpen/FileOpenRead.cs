using StarRailDamage.Source.Extension;
using System.IO;

namespace StarRailDamage.Source.Service.IO.FileOpen
{
    public class FileOpenRead : FileOpenStream
    {
        public FileOpenRead(string path)
        {
            Success = StreamExtension.TryOpenRead(path, out FileStream? _FileStream, this) && true.Configure(Stream = _FileStream);
        }

        public FileOpenRead(Stream stream) : base(stream) { }
    }
}