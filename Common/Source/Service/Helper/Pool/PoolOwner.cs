using System.Buffers;

namespace Common.Source.Service.Helper.Pool
{
    public readonly struct PoolOwner<T> : IMemoryOwner<T>
    {
        private readonly IMemoryOwner<T> MemoryOwner;

        public Memory<T> Memory { get; }

        public int Length => Memory.Length;

        public Span<T> Span => Memory.Span;

        private PoolOwner(IMemoryOwner<T> memoryOwner, Memory<T> memory)
        {
            MemoryOwner = memoryOwner;
            Memory = memory;
        }

        public static PoolOwner<T> Create(IMemoryOwner<T> owner, int length)
        {
            return new PoolOwner<T>(owner, owner.Memory[..length]);
        }

        public void Dispose()
        {
            MemoryOwner.Dispose();
        }
    }
}