using Common.Source.Extension;
using Common.Source.Model.DataStruct.Bitwise;

namespace Common.Source.Service.Encode.QRCode.Metadata
{
    public readonly struct ECCodeGroup
    {
        public int ECCodePerBytes { get; }

        public int BlocksInGroup1 { get; }

        public int CodewordsInGroup1 { get; }

        public int BlocksInGroup2 { get; }

        public int CodewordsInGroup2 { get; }

        private ECCodeGroup(int codewordsInGroup2, int blocksInGroup2, int codewordsInGroup1, int blocksInGroup1, int eccodePerBytes)
        {
            ECCodePerBytes = eccodePerBytes;
            BlocksInGroup1 = blocksInGroup1;
            CodewordsInGroup1 = codewordsInGroup1;
            BlocksInGroup2 = blocksInGroup2;
            CodewordsInGroup2 = codewordsInGroup2;
        }

        public static ECCodeGroup Create(int bits)
        {
            return new ECCodeGroup(new BitStack<int>(bits).OutSender(out BitStack<int> bitStack).Captured(bitStack.Pop(7)), bitStack.Pop(7), bitStack.Pop(7), bitStack.Pop(6), bitStack.Pop(5));
        }
    }
}