using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct;

namespace StarRailDamage.Source.Service.Encode.QRCode
{
    public abstract class QRCodeEncoder
    {
        public int Version { get; set; }

        public abstract EncodeMode EncodeMode { get; }

        public ECCodeLevel ECCodeLevel { get; set; }

        public ECCodeGroup ECCodeGroup { get; private set; }

        public int Capacity { get; private set; }

        protected abstract int BitsOfContent { get; }

        protected abstract BitSet BinaryEncode(ReadOnlySpan<byte> content);

        protected abstract int GetUsedBitCount(int length);

        protected abstract int[,] GetCapacityTable();

        public int GetMaxCapacity()
        {
            return GetCapacityTable()[Version - 1, ECCodeLevel.ToInt()];
        }

        public void SetOptimalVersion(ReadOnlySpan<byte> content, int value = 0)
        {
            int Level = ECCodeLevel.ToInt();
            int[,] CapacityTable = GetCapacityTable();
            int Neutral = CapacityTable.BinarySearch(0, CapacityTable.GetLength(0), (Self, Index) => CapacityTable[Index, Level], content.Length).GetNeutral();
            Version = Math.Clamp(Neutral + 1, value, 40);
        }

        public void Complete()
        {
            Capacity = QRCodeInfo.GetCapacity(Version, ECCodeLevel);
            ECCodeGroup = ECCodeInfo.GetECCodeInfo(Version, ECCodeLevel);
        }

        public byte[] Encode(ReadOnlySpan<byte> content)
        {
            int Offset = 0;
            BitSet EncodedData = ContentEncode(content);
            int TotalBlockCount = ECCodeGroup.BlocksInGroup1 + ECCodeGroup.BlocksInGroup2;
            byte[][] Content = new byte[TotalBlockCount][];
            byte[] RSEncode(int index, int length)
            {
                Span<byte> SubData = EncodedData.Bytes.AsSpan(Offset, length);
                Content[index] = SubData.ToArray();
                return XPolynom.RSEncode(SubData, ECCodeGroup.ECCodePerBytes);
            }
            byte[][] ECCode = new byte[TotalBlockCount][];
            for (int i = 0; i < ECCodeGroup.BlocksInGroup1; i++, Offset += ECCodeGroup.CodewordsInGroup1)
            {
                ECCode[i] = RSEncode(i, ECCodeGroup.CodewordsInGroup1);
            }
            for (int i = ECCodeGroup.BlocksInGroup1; i < TotalBlockCount; i++, Offset += ECCodeGroup.CodewordsInGroup2)
            {
                ECCode[i] = RSEncode(i, ECCodeGroup.CodewordsInGroup2);
            }
            return Interlock(Content, ECCode);
        }

        public BitSet ContentEncode(ReadOnlySpan<byte> content)
        {
            BitSet Binary = BinaryEncode(content);
            BitSet FinalContent = BitSet.FromBitCount(Capacity * 8);
            FinalContent.Write(0, 1 << (EncodeMode.ToInt() - 1), 4);
            FinalContent.Write(4, content.Length, BitsOfContent);
            FinalContent.Write(4 + BitsOfContent, Binary, 0, Binary.Count);
            int Index = (GetTotalBitCount(content.Length) + 7) & ~7;
            while (Index < FinalContent.Count)
            {
                FinalContent.Write(Index, 0b11101100, 8);
                if ((Index += 8) < FinalContent.Count)
                {
                    FinalContent.Write(Index, 0b00010001, 8);
                    Index += 8;
                }
            }
            return FinalContent;
        }

        public int GetTotalBitCount(int length)
        {
            int UsedBitCount = 4 + BitsOfContent + GetUsedBitCount(length);
            return UsedBitCount + Math.Min(4, Capacity * 8 - UsedBitCount);
        }

        public byte[] Interlock(byte[][] content, byte[][] eccode)
        {
            int Offset = -1;
            int ECCodeCount = ECCodeGroup.ECCodePerBytes * eccode.Length;
            int ContentCount = ECCodeGroup.BlocksInGroup1 * ECCodeGroup.CodewordsInGroup1 + ECCodeGroup.BlocksInGroup2 * ECCodeGroup.CodewordsInGroup2;
            byte[] FinalContent = new byte[ContentCount + ECCodeCount];
            for (int x = 0; x < ECCodeGroup.CodewordsInGroup1; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    FinalContent[++Offset] = content[y][x];
                }
            }
            for (int x = ECCodeGroup.CodewordsInGroup1; x < ECCodeGroup.CodewordsInGroup2; x++)
            {
                for (int y = ECCodeGroup.BlocksInGroup1; y < content.Length; y++)
                {
                    FinalContent[++Offset] = content[y][x];
                }
            }
            for (int x = 0; x < ECCodeGroup.ECCodePerBytes; x++)
            {
                for (int y = 0; y < eccode.Length; y++)
                {
                    FinalContent[++Offset] = eccode[y][x];
                }
            }
            return FinalContent;
        }
    }
}