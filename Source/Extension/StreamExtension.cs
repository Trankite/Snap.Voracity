using StarRailDamage.Source.Core.Abstraction;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.ExceptionServices;

namespace StarRailDamage.Source.Extension
{
    public static class StreamExtension
    {
        [DebuggerStepThrough]
        public static bool TryGetStreamReader(this Stream stream, [NotNullWhen(true)] out StreamReader? streamReader, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(streamReader = new StreamReader(stream));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(streamReader = null);
            }
        }

        [DebuggerStepThrough]
        public static bool TryGetStreamReader(string path, [NotNullWhen(true)] out StreamReader? streamReader, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(streamReader = new StreamReader(path));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(streamReader = null);
            }
        }

        [DebuggerStepThrough]
        public static bool TryGetStreamWriter(Stream stream, [NotNullWhen(true)] out StreamWriter? streamWriter, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(streamWriter = new StreamWriter(stream));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(streamWriter = null);
            }
        }

        [DebuggerStepThrough]
        public static bool TryGetStreamWriter(string path, [NotNullWhen(true)] out StreamWriter? streamWriter, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(streamWriter = new StreamWriter(path));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(streamWriter = null);
            }
        }

        [DebuggerStepThrough]
        public static bool TryOpenRead(string path, [NotNullWhen(true)] out FileStream? stream, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(stream = File.OpenRead(path));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(stream = null);
            }
        }

        [DebuggerStepThrough]
        public static bool TryOpenWrite(string path, [NotNullWhen(true)] out FileStream? stream, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(stream = File.OpenWrite(path));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(stream = null);
            }
        }

        [DebuggerStepThrough]
        public static bool TryOpen(string path, [NotNullWhen(true)] out FileStream? stream, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, IExceptionCapture? exceptionCapture = null)
        {
            try
            {
                return true.Configure(stream = File.Open(path, fileMode, fileAccess));
            }
            catch (Exception Exception)
            {
                return false.Configure(exceptionCapture?.Exception = ExceptionDispatchInfo.Capture(Exception)).Configure(stream = null);
            }
        }
    }
}