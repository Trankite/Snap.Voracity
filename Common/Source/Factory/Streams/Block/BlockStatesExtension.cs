namespace Common.Source.Factory.Streams.Block
{
    public static class BlockStatesExtension
    {
        public static bool IsComplete(this BlockStates state)
        {
            return (state & (BlockStates.Finish | BlockStates.Ending)) > 0;
        }
    }
}