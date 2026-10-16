using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Model.DataStruct.Ticket;
using System.Diagnostics;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public static class BlockReaderExtension
    {
        [DebuggerStepThrough]
        public static bool TryRead<T>(this IBlockReader<T> blockReader, out ReadOnlyMemory<T> blockMemory)
        {
            return !blockReader.ReadBlock().OutSelf(out blockMemory).IsEmpty;
        }

        [DebuggerStepThrough]
        public static async ValueTask<CheckTicket<ReadOnlyMemory<T>>> ReadAsync<T>(this IAsyncBlockReader<T> blockReader, CancellationToken cancellationToken = default)
        {
            ReadOnlyMemory<T> BlockMemory = await blockReader.ReadBlockAsync(cancellationToken).ConfigureAwait(false);
            return CheckTicket.Create(!BlockMemory.IsEmpty, BlockMemory);
        }
    }
}