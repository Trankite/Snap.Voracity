using Common.Source.Model.DataStruct.Disk;
using Common.Source.Model.DataStruct.Disk.Metadata;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Common.Source.Service.Pool
{
    public static class BufferPool<T>
    {
        private static readonly int DefaultSize;

        public static PoolOwner<T> GetBuffer(int length)
        {
            return PoolOwner<T>.Create(MemoryPool<T>.Shared.Rent(length), 0, length);
        }

        public static PoolOwner<T> GetBuffer() => GetBuffer(DefaultSize);

        static BufferPool()
        {
            DefaultSize = Convert.ToInt32(DiskSize.Create(DataSize.KB, 4).Bytes / Unsafe.SizeOf<T>());
        }
    }
}