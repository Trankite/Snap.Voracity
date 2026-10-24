namespace Common.Source.Service.Encode.QRCode
{
    public readonly struct ECCodeGroup
    {
        public int ECCodePerBytes { get; }

        public int BlocksInGroup1 { get; }

        public int CodewordsInGroup1 { get; }

        public int BlocksInGroup2 { get; }

        public int CodewordsInGroup2 { get; }

        public ECCodeGroup(int eCCodePerBytes, int blocksInGroup1, int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
        {
            ECCodePerBytes = eCCodePerBytes;
            BlocksInGroup1 = blocksInGroup1;
            CodewordsInGroup1 = codewordsInGroup1;
            BlocksInGroup2 = blocksInGroup2;
            CodewordsInGroup2 = codewordsInGroup2;
        }
    }
}