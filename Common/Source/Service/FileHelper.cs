using Common.Source.Extension;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service
{
    public static class FileHelper
    {
        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(path))]
        public static string? BuildPath(string? path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try { Directory.CreateDirectory(path); } catch { }
            }
            return path;
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(path))]
        public static string? BuildFilePath(string? path)
        {
            return BuildPath(Path.GetDirectoryName(path));
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(path))]
        public static string? PathOpen(string? path)
        {
            if (OperatingSystem.IsWindows())
            {
                using Process WindowsOpen = Process.Start("explorer", File.Exists(path) ? $"/select,\"{path}\"" : $"\"{path}\"");
            }
            else if (OperatingSystem.IsMacOS())
            {
                using Process MacOpen = Process.Start("open", $"-R \"{path}\"");
            }
            return path;
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(path))]
        public static string? PathOpen(string? path, bool flag)
        {
            return flag ? PathOpen(path) : path;
        }

        [DebuggerStepThrough]
        public static string GetExtensionName(string? path)
        {
            return Path.GetExtension(path).Captured(Extension => Extension.IsNotNull() && Extension.Length >= 1 ? Extension[1..] : string.Empty);
        }
    }
}