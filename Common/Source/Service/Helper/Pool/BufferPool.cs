using Common.Source.Model.DataStruct;
using Common.Source.Model.DataStruct.Disk;
using Common.Source.Model.DataStruct.Disk.Metadata;
using System.Buffers;
using System.Diagnostics;

namespace Common.Source.Service.Helper.Pool
{
    public static class BufferPool<T>
    {
        private static readonly int DefaultSize;

        [StackTraceHidden]
        public static PoolOwner<T> GetBuffer(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
            return PoolOwner<T>.Create(MemoryPool<T>.Shared.Rent(length), length);
        }

        [StackTraceHidden]
        public static PoolOwner<T> GetAutoBuffer(long length = -1)
        {
            return GetBuffer(length < DefaultSize && length >= 0 ? (int)length : DefaultSize);
        }

        static BufferPool()
        {
            DefaultSize = (int)DiskSize.Create(DataSize.KiB, 4).Bytes / TypeSize<T>.Size;
        }
    }
}