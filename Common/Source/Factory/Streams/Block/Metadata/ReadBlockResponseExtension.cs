using Common.Source.Extension;

namespace Common.Source.Factory.Streams.Block.Metadata
{
    public static class ReadBlockResponseExtension
    {
        public static bool ReadBlock<T>(this ReadBlockResponse<T> response, out ReadOnlyMemory<T> span)
        {
            return response.IsCanRead.Configure(span = response.BlockSpan);
        }
    }
}