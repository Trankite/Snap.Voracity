using Common.Source.Extension;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service
{
    public static class FileHelper
    {
        private static readonly HashSet<char> InvalidFileNameTable;

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(filePath))]
        public static string? BuildPath(string? filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                try { Directory.CreateDirectory(filePath); } catch { }
            }
            return filePath;
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(filePath))]
        public static string? BuildFilePath(string? filePath)
        {
            return BuildPath(Path.GetDirectoryName(filePath));
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(filePath))]
        public static string? PathOpen(string? filePath)
        {
            if (OperatingSystem.IsWindows())
            {
                using Process WindowsOpen = Process.Start("explorer", File.Exists(filePath) ? $"/select,\"{filePath}\"" : $"\"{filePath}\"");
            }
            else if (OperatingSystem.IsMacOS())
            {
                using Process MacOpen = Process.Start("open", $"-R \"{filePath}\"");
            }
            return filePath;
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(filePath))]
        public static string? PathOpen(string? filePath, bool flag)
        {
            return flag ? PathOpen(filePath) : filePath;
        }

        [DebuggerStepThrough]
        public static string GetExtensionName(string? filePath)
        {
            return Path.GetExtension(filePath).Captured(Extension => Extension.IsNotNull() && Extension.Length >= 1 ? Extension[1..] : string.Empty);
        }

        [DebuggerStepThrough]
        public static string GetSafeFileName(string filePath)
        {
            return new string(filePath.Where(Current => !InvalidFileNameTable.Contains(Current)).ToArray().AsSpan().Trim());
        }

        static FileHelper()
        {
            InvalidFileNameTable = [.. Path.GetInvalidFileNameChars()];
        }
    }
}