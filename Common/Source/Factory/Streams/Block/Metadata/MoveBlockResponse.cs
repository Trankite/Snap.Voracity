namespace Common.Source.Factory.Streams.Block.Metadata
{
    public readonly struct MoveBlockResponse<T>
    {
        public BlockStates State { get; }

        public ReadOnlyMemory<T> BlockSpan { get; }

        public MoveBlockResponse(BlockStates state, ReadOnlyMemory<T> blockSpan)
        {
            State = state;
            BlockSpan = blockSpan;
        }
    }
}