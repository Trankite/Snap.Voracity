using System.Diagnostics;

namespace Common.Source.Factory.Streams.Block.Metadata
{
    public static class BlockStatesExtension
    {
        [DebuggerStepThrough]
        public static bool IsComplete(this BlockStates state)
        {
            return (state & (BlockStates.Finish | BlockStates.Ending)) > 0;
        }
    }
}