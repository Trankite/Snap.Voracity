using StarRailDamage.Source.Extension;
using System.IO;

namespace StarRailDamage.Source.Service.IO.FileOpen
{
    public class FileOpenWrite : FileOpenStream
    {
        public FileOpenWrite(string path)
        {
            Success = StreamExtension.TryOpenWrite(path, out FileStream? _FileStream, this) && true.Configure(Stream = _FileStream);
        }

        public FileOpenWrite(Stream stream) : base(stream) { }
    }
}