using Common.Source.Extension;

namespace Common.Source.Factory.Streams.Block.Metadata
{
    public static class MoveBlockResponseExtension
    {
        public static BlockStates MoveNext<T>(this MoveBlockResponse<T> response, out ReadOnlyMemory<T> span)
        {
            return response.State.Configure(span = response.BlockSpan);
        }
    }
}