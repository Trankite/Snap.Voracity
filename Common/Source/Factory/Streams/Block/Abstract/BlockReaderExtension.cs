using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Factory.Streams.Block.Metadata;
using System.Diagnostics;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public static class BlockReaderExtension
    {
        [DebuggerStepThrough]
        public static bool TryRead<T>(this IBlockReader<T> blockReader, out ReadOnlyMemory<T> span)
        {
            return blockReader.ReadBlock().OutSelf(out ReadBlockResponse<T> Response).IsCanRead.Configure(span = Response.BlockSpan);
        }
    }
}