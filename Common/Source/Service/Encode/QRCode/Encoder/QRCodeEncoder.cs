using Common.Source.Extension;
using Common.Source.Model.Abstract;
using Common.Source.Model.Collection;
using Common.Source.Model.DataStruct;
using Common.Source.Model.Exceptions;
using Common.Source.Service.Encode.QRCode.Metadata;

namespace Common.Source.Service.Encode.QRCode.Encoder
{
    public abstract class QRCodeEncoder : Freezable
    {
        private static readonly uint[][] ECCodeGroupTable;

        private static readonly SortedArray<short>[] QRCodeCapacityTable;

        public int Capacity { get; private set; }

        public int Version
        {
            get; set => SetValue(ref field, value);
        }

        public ECCodeLevel ECCodeLevel
        {
            get; set => SetValue(ref field, value);
        }

        public ECCodeGroup ECCodeGroup { get; private set; }

        public abstract EncodeMode EncodeMode { get; }

        protected abstract int BitsOfContent { get; }

        protected abstract BitSet BinaryEncode(ReadOnlySpan<byte> content);

        protected abstract int GetUsedBitCount(int length);

        protected abstract SortedArray<short> GetCapacityTable(ECCodeLevel eccodeLevel);

        public int GetMaxCapacity()
        {
            return GetCapacityTable(ECCodeLevel)[Version - 1];
        }

        public void SetOptimalVersion(ReadOnlySpan<byte> content, int value = 0)
        {
            ObjectFrozenException.ThrowIfFrozen(this);
            int index = GetCapacityTable(ECCodeLevel).Search((short)content.Length).GetInsert();
            Version = Math.Clamp(index + 1, value, 40);
        }

        protected override void FreezeOverride()
        {
            Capacity = QRCodeCapacityTable[(int)ECCodeLevel][Version - 1];
            ECCodeGroup = ECCodeGroup.Create(ECCodeGroupTable[(int)ECCodeLevel][Version - 1]);
        }

        public byte[] Encode(ReadOnlySpan<byte> content)
        {
            int offset = 0;
            BitSet encodedData = ContentEncode(content);
            int totalBlockCount = ECCodeGroup.BlocksInGroup1 + ECCodeGroup.BlocksInGroup2;
            byte[][] encodedContent = new byte[totalBlockCount][];
            byte[] RSEncode(int index, int length)
            {
                Span<byte> submitData = encodedData.Bytes.AsSpan(offset, length);
                encodedContent[index] = submitData.ToArray();
                return GaloisField.RSEncode(submitData, ECCodeGroup.ECCodePerBytes);
            }
            byte[][] eccode = new byte[totalBlockCount][];
            for (int i = 0; i < ECCodeGroup.BlocksInGroup1; i++, offset += ECCodeGroup.CodewordsInGroup1)
            {
                eccode[i] = RSEncode(i, ECCodeGroup.CodewordsInGroup1);
            }
            for (int i = ECCodeGroup.BlocksInGroup1; i < totalBlockCount; i++, offset += ECCodeGroup.CodewordsInGroup2)
            {
                eccode[i] = RSEncode(i, ECCodeGroup.CodewordsInGroup2);
            }
            return Interlock(encodedContent, eccode);
        }

        private BitSet ContentEncode(ReadOnlySpan<byte> content)
        {
            BitSet binary = BinaryEncode(content);
            BitSet encodedContent = BitSet.Create(Capacity * 8);
            encodedContent.Write(0, 1 << ((int)EncodeMode - 1), 4);
            encodedContent.Write(4, content.Length, BitsOfContent);
            encodedContent.Write(4 + BitsOfContent, binary, 0, binary.Count);
            int index = (GetTotalBitCount(content.Length) + 7) & ~7;
            while (index < encodedContent.Count)
            {
                encodedContent.Write(index, 0b11101100, 8);
                if ((index += 8) < encodedContent.Count)
                {
                    encodedContent.Write(index, 0b00010001, 8);
                    index += 8;
                }
            }
            return encodedContent;
        }

        public int GetTotalBitCount(int length)
        {
            int usedBitCount = 4 + BitsOfContent + GetUsedBitCount(length);
            return usedBitCount + Math.Min(4, Capacity * 8 - usedBitCount);
        }

        private byte[] Interlock(byte[][] content, byte[][] eccode)
        {
            int offset = -1;
            int eccodeCount = ECCodeGroup.ECCodePerBytes * eccode.Length;
            int contentCount = ECCodeGroup.BlocksInGroup1 * ECCodeGroup.CodewordsInGroup1 + ECCodeGroup.BlocksInGroup2 * ECCodeGroup.CodewordsInGroup2;
            byte[] encodedContent = new byte[contentCount + eccodeCount];
            for (int x = 0; x < ECCodeGroup.CodewordsInGroup1; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    encodedContent[++offset] = content[y][x];
                }
            }
            for (int x = ECCodeGroup.CodewordsInGroup1; x < ECCodeGroup.CodewordsInGroup2; x++)
            {
                for (int y = ECCodeGroup.BlocksInGroup1; y < content.Length; y++)
                {
                    encodedContent[++offset] = content[y][x];
                }
            }
            for (int x = 0; x < ECCodeGroup.ECCodePerBytes; x++)
            {
                for (int y = 0; y < eccode.Length; y++)
                {
                    encodedContent[++offset] = eccode[y][x];
                }
            }
            return encodedContent;
        }

        static QRCodeEncoder()
        {
            QRCodeCapacityTable =
            [
                new SortedArray<short>([19, 34, 55, 80, 108, 136, 156, 194, 232, 274, 324, 370, 428, 461, 523, 589, 647, 721, 795, 861, 932, 1006, 1094, 1174, 1276, 1370, 1468, 1531, 1631, 1735, 1843, 1955, 2071, 2191, 2306, 2434, 2566, 2702, 2812, 2956]),
                new SortedArray<short>([16, 28, 44, 64, 86,  108, 124, 154, 182, 216, 254, 290, 334, 365, 415, 453, 507, 563, 627, 669, 714, 782,  860,  914,  1000, 1062, 1128, 1193, 1267, 1373, 1455, 1541, 1631, 1725, 1812, 1914, 1992, 2102, 2216, 2334]),
                new SortedArray<short>([13, 22, 34, 48, 62,  76,  88,  110, 132, 154, 180, 206, 244, 261, 295, 325, 367, 397, 445, 485, 512, 568,  614,  664,  718,  754,  808,  871,  911,  985,  1033, 1115, 1171, 1231, 1286, 1354, 1426, 1502, 1582, 1666]),
                new SortedArray<short>([9,  16, 26, 36, 46,  60,  66,  86,  100, 122, 140, 158, 180, 197, 223, 253, 283, 313, 341, 385, 406, 442,  464,  514,  538,  596,  628,  661,  701,  745,  793,  845,  901,  961,  986,  1054, 1096, 1142, 1222, 1276])
            ];
            ECCodeGroupTable =
            [
                [0x3824C000, 0x50288000, 0x782DC000, 0xA0340000, 0xD03B0000, 0x90510000, 0xA0538000, 0xC0584000, 0xF05D0000, 0x90510145, 0xA0944000, 0xC057015D, 0xD09AC000, 0xF07CC0F4, 0xB0B5C0D8, 0xC0B880E3, 0xE03AC2EC, 0xF0BE00F9, 0xE07C4272, 0xE07AC2EC, 0xE09D0275, 0xE05BC3F0, 0xF09E42FA, 0xF0DD4276, 0xD11A826B, 0xE15C8173, 0xF11E827B, 0xF07D4576, 0xF0FD03F5, 0xF0BCC574, 0xF1BCC1F4, 0xF23CC000, 0xF23CC0F4, 0xF1BCC374, 0xF19E43FA, 0xF0DE477A, 0xF23E827B, 0xF09E897B, 0xF29D4276, 0xF27D8377],
                [0x50240000, 0x80270000, 0xD02B0000, 0x90480000, 0xC04AC000, 0x8086C000, 0x9087C000, 0xB0498127, 0xB0690125, 0xD08AC0AC, 0xF02C8233, 0xB0C90125, 0xB10940A6, 0xC08A02A9, 0xC0AA42AA, 0xE0EB41AE, 0xE14B80AF, 0xD12AC22C, 0xD06B05AD, 0xD06A46AA, 0xD22A8000, 0xE22B8000, 0xE08BC730, 0xE0CB472E, 0xE10BC6B0, 0xE26B822F, 0xE2CB41AE, 0xE06B4BAE, 0xE2AB43AE, 0xE26BC530, 0xE04B8EAF, 0xE14B8BAF, 0xE1CB8AAF, 0xE1CB8BAF, 0xE18BCD30, 0xE0CBD130, 0xE3AB872F, 0xE1AB902F, 0xE50BC3B0, 0xE24BCFB0],
                [0x68234000, 0xB0258000, 0x90444000, 0xD0460000, 0x9043C110, 0xC084C000, 0x9043820F, 0xB0848113, 0xA0840211, 0xC0C4C114, 0xE0858217, 0xD0850315, 0xC1050215, 0xA1640291, 0xF0A60399, 0xC1E4C114, 0xE0258797, 0xE2258097, 0xD2254216, 0xF1E60299, 0xE2258317, 0xF0E60819, 0xF1660719, 0xF1660819, 0xF0E60B19, 0xE3858317, 0xF105CD18, 0xF0860F99, 0xF025D298, 0xF1E60C99, 0xF5460099, 0xF1461199, 0xF3A60999, 0xF5860399, 0xF4E60719, 0xF5C60519, 0xF6260519, 0xF6060719, 0xF5660B19, 0xF4461119],
                [0x88224000, 0xE0240000, 0xB0434000, 0x80824000, 0xB042C10C, 0xE083C000, 0xD083408E, 0xD083810F, 0xC083020D, 0xE0C3C110, 0xC063040D, 0xE0E3820F, 0xB182C20C, 0xC163028D, 0xC163038D, 0xF063C690, 0xE043888F, 0xE043898F, 0xD123480E, 0xE1E3C510, 0xF2640311, 0xC4434000, 0xF203C710, 0xF3C40111, 0xF2C3C690, 0xF4240211, 0xF183CE10, 0xF163CF90, 0xF263CD10, 0xF2E3CC90, 0xF2E3CE10, 0xF263D190, 0xF163D710, 0xF7640091, 0xF2C3D490, 0xF043E010, 0xF303D710, 0xF543D010, 0xF143E190, 0xF283DE90]
            ];
        }
    }
}