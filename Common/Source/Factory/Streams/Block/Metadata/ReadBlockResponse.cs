namespace Common.Source.Factory.Streams.Block.Metadata
{
    public readonly struct ReadBlockResponse<T>
    {
        public bool IsCanRead { get; }

        public ReadOnlyMemory<T> BlockSpan { get; }

        public ReadBlockResponse(bool isCanRead, ReadOnlyMemory<T> blockSpan)
        {
            IsCanRead = isCanRead;
            BlockSpan = blockSpan;
        }
    }
}