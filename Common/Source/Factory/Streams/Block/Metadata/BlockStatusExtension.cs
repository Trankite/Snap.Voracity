using System.Diagnostics;

namespace Common.Source.Factory.Streams.Block.Metadata
{
    public static class BlockStatusExtension
    {
        [DebuggerStepThrough]
        public static bool IsComplete(this BlockStatus state)
        {
            return (state & (BlockStatus.Finish | BlockStatus.Ending)) > 0;
        }
    }
}