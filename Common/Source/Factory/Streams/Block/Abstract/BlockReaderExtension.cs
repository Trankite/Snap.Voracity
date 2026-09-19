using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Factory.Streams.Block.Metadata;
using Common.Source.Model.DataStruct.Ticket;
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

        [DebuggerStepThrough]
        public static async ValueTask<CheckTicket<ReadOnlyMemory<T>>> ReadAsync<T>(this IAsyncBlockReader<T> blockReader, CancellationToken cancellationToken = default)
        {
            return CheckTicket.Create((await blockReader.ReadBlockAsync(cancellationToken).ConfigureAwait(false)).OutSelf(out ReadBlockResponse<T> Response).IsCanRead, Response.BlockSpan);
        }
    }
}