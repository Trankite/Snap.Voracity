using Common.Source.Model.DataStruct;

namespace Common.Source.Service.Encode.QRCode
{
    public readonly struct ECCodeGroup
    {
        public int ECCodePerBytes { get; }

        public int BlocksInGroup1 { get; }

        public int CodewordsInGroup1 { get; }

        public int BlocksInGroup2 { get; }

        public int CodewordsInGroup2 { get; }

        public ECCodeGroup(int eccodePerBytes, int blocksInGroup1, int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
        {
            ECCodePerBytes = eccodePerBytes;
            BlocksInGroup1 = blocksInGroup1;
            CodewordsInGroup1 = codewordsInGroup1;
            BlocksInGroup2 = blocksInGroup2;
            CodewordsInGroup2 = codewordsInGroup2;
        }

        public static ECCodeGroup Create(uint bits)
        {
            Bitwise<int> bitwise = new((int)bits);
            int eccodePerBytes = bitwise.HighPop(5);
            int blocksInGroup1 = bitwise.HighPop(6);
            int codewordsInGroup1 = bitwise.HighPop(7);
            int blocksInGroup2 = bitwise.HighPop(7);
            int codewordsInGroup2 = bitwise.HighPop(7);
            return new ECCodeGroup(eccodePerBytes, blocksInGroup1, codewordsInGroup1, blocksInGroup2, codewordsInGroup2);
        }
    }
}