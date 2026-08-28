using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using System.Text;

namespace Common.Source.Factory.Streams.Block
{
    public static class BlockStreamExtension
    {
        public static bool ReadToEnd<T>(this IReadOnlyBlockStream<T> blockStream, T element, Action<int, BlockStates, ReadOnlySpan<T>> action)
        {
            BlockStates State = default;
            for (int i = 0; !State.IsComplete(); i++)
            {
                action(i, State = blockStream.MoveNext(element, out ReadOnlySpan<T> Block), Block);
            }
            return State != BlockStates.Ending;
        }

        public static StringBuilder ReadToEnd(this IReadOnlyBlockStream<char> blockStream, char element, StringBuilder? stringBuilder = default)
        {
            return (stringBuilder ??= new StringBuilder()).Configure(blockStream.ReadToEnd(element, (_, _, block) => stringBuilder.Append(block)));
        }
    }
}