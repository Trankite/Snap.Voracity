using StarRailDamage.Source.Extension;
using System.Diagnostics;
using System.IO;

namespace StarRailDamage.Source.Service.IO
{
    public static class FileHelper
    {
        [DebuggerStepThrough]
        public static string BuildPath(string? path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try { Directory.CreateDirectory(path); } catch { }
            }
            return path ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string BuildFilePath(string? path)
        {
            BuildPath(Path.GetDirectoryName(path));
            return path ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string PathOpen(string? path)
        {
            return path.Configure(Process.Start("explorer", $"{(File.Exists(path) ? "/select," : string.Empty)}\"{path}\"")) ?? string.Empty;
        }
    }
}