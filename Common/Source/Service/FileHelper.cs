using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Service
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
            return path.NotNull();
        }

        [DebuggerStepThrough]
        public static string BuildFilePath(string? path)
        {
            return BuildPath(Path.GetDirectoryName(path)).Captured(path.NotNull());
        }

        [DebuggerStepThrough]
        public static string PathOpen(string? path)
        {
            return path.Configure(Process.Start("explorer", $"{(File.Exists(path) ? "/select," : string.Empty)}\"{path}\"")).NotNull();
        }

        [DebuggerStepThrough]
        public static string PathOpen(string? path, bool flag)
        {
            return flag ? PathOpen(path) : path.NotNull();
        }

        [DebuggerStepThrough]
        public static string GetExtensionName(string? path)
        {
            return Path.GetExtension(path).Captured(Extension => Extension.IsNotNull() && Extension.Length >= 1 ? Extension[1..] : string.Empty);
        }
    }
}