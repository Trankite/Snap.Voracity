namespace Common.Source.Factory.Streams.Block
{
    [Flags]
    public enum BlockStates
    {
        Await = 0x01,
        Finish = 0x02,
        Ending = 0x04
    }
}