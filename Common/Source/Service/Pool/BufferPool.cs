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
            DefaultSize = 4 * 1024 / Unsafe.SizeOf<T>();
        }
    }
}