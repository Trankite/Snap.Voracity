namespace Common.Source.Factory.Streams.Block.Metadata
{
    [Flags]
    public enum BlockStatus
    {
        Await = 0x01,
        Finish = 0x02,
        Ending = 0x04
    }
}