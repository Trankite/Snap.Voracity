using Common.Source.Model.Collection;
using Common.Source.Model.DataStruct.Span;
using Common.Source.Service.Encode.QRCode.Metadata;

namespace Common.Source.Service.Encode.QRCode.Encoder
{
    internal class NumericEncoder : QRCodeEncoder
    {
        private static readonly int[] SurplusTable = [0, 4, 7];

        private static readonly SortedArray<int>[] CapacityTable;

        public override EncodeMode EncodeMode => EncodeMode.Numeric;

        protected override SortedArray<int> GetCapacityTable(ECCodeLevel eccodeLevel)
        {
            return CapacityTable[(int)eccodeLevel];
        }

        protected override int BitsOfContent => Version >= 10 ? Version >= 27 ? 14 : 12 : 10;

        protected override int GetUsedBitCount(int length)
        {
            return length / 3 * 10 + SurplusTable[length % 3];
        }

        protected override BitSet BinaryEncode(ReadOnlySpan<byte> content)
        {
            int Count = content.Length;
            int BitCount = Count / 3;
            BitSet Result = BitSet.FromBitCount(GetUsedBitCount(Count));
            for (int i = 0; i < BitCount; i++)
            {
                Result.Write(i * 10, Encode(content[i * 3]) * 100 + Encode(content[i * 3 + 1]) * 10 + Encode(content[i * 3 + 2]), 10);
            }
            if (Count % 3 == 1)
            {
                Result.Write(BitCount * 10, Encode(content[BitCount * 3]), 4);
            }
            else if (Count % 3 == 2)
            {
                Result.Write(BitCount * 10, Encode(content[BitCount * 3]) * 10 + Encode(content[BitCount * 3 + 1]), 7);
            }
            return Result;
        }

        private static int Encode(byte value) => value - '0';

        static NumericEncoder()
        {
            CapacityTable =
            [
                new SortedArray<int>([41, 77, 127, 187, 255, 322, 370, 461, 552, 652, 772, 883, 1022, 1101, 1250, 1408, 1548, 1725, 1903, 2061, 2232, 2409, 2620, 2812, 3057, 3283, 3517, 3669, 3909, 4158, 4417, 4686, 4965, 5253, 5529, 5836, 6153, 6479, 6743, 7089]),
                new SortedArray<int>([34, 63, 101, 149, 202, 255, 293, 365, 432, 513, 604, 691, 796,  871,  991,  1082, 1212, 1346, 1500, 1600, 1708, 1872, 2059, 2188, 2395, 2544, 2701, 2857, 3035, 3289, 3486, 3693, 3909, 4134, 4343, 4588, 4775, 5039, 5313, 5596]),
                new SortedArray<int>([27, 48, 77,  111, 144, 178, 207, 259, 312, 364, 427, 489, 580,  621,  703,  775,  876,  948,  1063, 1159, 1224, 1358, 1468, 1588, 1718, 1804, 1933, 2085, 2181, 2358, 2473, 2670, 2805, 2949, 3081, 3244, 3417, 3599, 3791, 3993]),
                new SortedArray<int>([17, 34, 58,  82,  106, 139, 154, 202, 235, 288, 331, 374, 427,  468,  530,  602,  674,  746,  813,  919,  969,  1056, 1108, 1228, 1286, 1425, 1501, 1581, 1677, 1782, 1897, 2022, 2157, 2301, 2361, 2524, 2625, 2735, 2927, 3057])
            ];
        }
    }
}