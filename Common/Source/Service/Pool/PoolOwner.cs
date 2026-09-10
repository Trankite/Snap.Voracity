using System.Buffers;

namespace Common.Source.Service.Pool
{
    public readonly struct PoolOwner<T> : IMemoryOwner<T>
    {
        private readonly IMemoryOwner<T> Owner;

        public Memory<T> Memory { get; }

        public int Length => Memory.Length;

        public Span<T> Span => Memory.Span;

        private PoolOwner(IMemoryOwner<T> owner, Memory<T> memory)
        {
            Owner = owner;
            Memory = memory;
        }

        public static PoolOwner<T> Create(IMemoryOwner<T> owner, int start, int length)
        {
            return new PoolOwner<T>(owner, owner.Memory.Slice(start, length));
        }

        public void Dispose()
        {
            Owner.Dispose();
        }
    }
}