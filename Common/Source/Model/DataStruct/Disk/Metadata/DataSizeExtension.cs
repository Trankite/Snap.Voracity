using Common.Source.Extension;
using System.Diagnostics;
using System.Numerics;

namespace Common.Source.Model.DataStruct.Disk.Metadata
{
    public static class DataSizeExtension
    {
        [DebuggerStepThrough]
        public static long GetBytes(this DataSize dataSize, long value = 1)
        {
            return value << (10 * (int)dataSize);
        }

        [DebuggerStepThrough]
        public static long GetBytes(this DataSize dataSize, double value, MidpointRounding mode = default)
        {
            return (long)Math.Round(value * GetBytes(dataSize), mode);
        }

        [DebuggerStepThrough]
        public static double GetSize(this DataSize dataSize, long bytes)
        {
            return (double)bytes / GetBytes(dataSize);
        }

        [DebuggerStepThrough]
        public static DataSize GetDataSize(long bytes)
        {
            return (DataSize)(BitOperations.Log2(bytes.Abs()) / 10);
        }
    }
}