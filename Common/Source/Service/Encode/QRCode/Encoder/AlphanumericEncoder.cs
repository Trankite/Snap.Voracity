using Common.Source.Model.Collection;
using Common.Source.Model.DataStruct;
using Common.Source.Service.Encode.QRCode.Metadata;

namespace Common.Source.Service.Encode.QRCode.Encoder
{
    public sealed class AlphanumericEncoder : QRCodeEncoder
    {
        private static readonly int[] AlphanumericTable;

        private static readonly SortedArray<short>[] CapacityTable;

        public override EncodeMode EncodeMode => EncodeMode.Alphanumeric;

        protected override SortedArray<short> GetCapacityTable(ECCodeLevel eccodeLevel)
        {
            return CapacityTable[(int)eccodeLevel];
        }

        protected override int BitsOfContent => Version >= 10 ? Version >= 27 ? 13 : 11 : 9;

        protected override int GetUsedBitCount(int length)
        {
            return length / 2 * 11 + length % 2 * 6;
        }

        protected override BitSet BinaryEncode(ReadOnlySpan<byte> content)
        {
            int count = content.Length;
            int doubleCount = count / 2;
            BitSet result = BitSet.Create(GetUsedBitCount(count));
            for (int i = 0; i < doubleCount; i++)
            {
                result.Write(i * 11, Encode(content[2 * i]) * 45 + Encode(content[2 * i + 1]), 11);
            }
            if (count % 2 != 0)
            {
                result.Write(doubleCount * 11, Encode(content[2 * doubleCount]), 6);
            }
            return result;
        }

        private static int Encode(byte value) => AlphanumericTable[value - 0x20];

        public static bool IsValid(byte value)
        {
            return value >= 0x20 && value <= 'Z' && AlphanumericTable[value - 0x20] >= 0;
        }

        static AlphanumericEncoder()
        {
            int index = 0;
            AlphanumericTable = [.. Enumerable.Repeat(-1, 'Z' - 0x20 + 1)];
            for (int i = '0'; i <= '9'; i++)
            {
                AlphanumericTable[i - 0x20] = index++;
            }
            for (int i = 'A'; i <= 'Z'; i++)
            {
                AlphanumericTable[i - 0x20] = index++;
            }
            char[] otherChars = [(char)0x20, '$', '%', '*', '+', '-', '.', '/', ':'];
            for (int i = 0; i < otherChars.Length; i++)
            {
                AlphanumericTable[otherChars[i] - 0x20] = index++;
            }
            CapacityTable =
            [
                new SortedArray<short>([25, 47, 77, 114, 154, 195, 224, 279, 335, 395, 468, 535, 619, 667, 758, 854, 938, 1046, 1153, 1249, 1352, 1460, 1588, 1704, 1853, 1990, 2132, 2223, 2369, 2520, 2677, 2840, 3009, 3183, 3351, 3537, 3729, 3927, 4087, 4296]),
                new SortedArray<short>([20, 38, 61, 90,  122, 154, 178, 221, 262, 311, 366, 419, 483, 528, 600, 656, 734, 816,  909,  970,  1035, 1134, 1248, 1326, 1451, 1542, 1637, 1732, 1839, 1994, 2113, 2238, 2369, 2506, 2632, 2780, 2894, 3054, 3220, 3391]),
                new SortedArray<short>([16, 29, 47, 67,  87,  108, 125, 157, 189, 221, 259, 296, 352, 376, 426, 470, 531, 574,  644,  702,  742,  823,  890,  963,  1041, 1094, 1172, 1263, 1322, 1429, 1499, 1618, 1700, 1787, 1867, 1966, 2071, 2181, 2298, 2420]),
                new SortedArray<short>([10, 20, 35, 50,  64,  84,  93,  122, 143, 174, 200, 227, 259, 283, 321, 365, 408, 452,  493,  557,  587,  640,  672,  744,  779,  864,  910,  958,  1016, 1080, 1150, 1226, 1307, 1394, 1431, 1530, 1591, 1658, 1774, 1852])
            ];
        }
    }
}