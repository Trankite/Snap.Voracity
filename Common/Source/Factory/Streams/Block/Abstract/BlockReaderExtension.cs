using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Factory.Streams.Block.Metadata;
using System.Text;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public static class BlockReaderExtension
    {
        public static bool ReadContent<T>(this IBlockReader<T> blockStream, T element, Action<int, MoveBlockResponse<T>> action, IEqualityComparer<T>? comparer = default)
        {
            BlockStates State = default;
            for (int i = 0; !State.IsComplete(); i++)
            {
                action(i, blockStream.MoveNext(element, comparer).Configure(Self => State = Self.State));
            }
            return State != BlockStates.Ending;
        }

        public static StringBuilder ReadContent(this IBlockReader<char> blockStream, char element, StringBuilder? stringBuilder = default)
        {
            return (stringBuilder ??= new StringBuilder()).Configure(blockStream.ReadContent(element, (_, response) => stringBuilder.Append(response.BlockSpan)));
        }

        public static StringBuilder ReadContentTrim(this IBlockReader<char> blockStream, char element, StringBuilder? stringBuilder = default)
        {
            stringBuilder ??= new StringBuilder();
            void Trim(int index, MoveBlockResponse<char> response)
            {
                ReadOnlyMemory<char> TrimSpan = response.BlockSpan;
                if (response.State.IsComplete())
                {
                    TrimSpan = TrimSpan.TrimEnd();
                }
                if (index == 0)
                {
                    TrimSpan = TrimSpan.TrimStart();
                }
                stringBuilder.Append(TrimSpan);
            }
            blockStream.ReadContent(element, Trim);
            return stringBuilder;
        }
    }
}