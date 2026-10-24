using Common.Source.Extension;
using Common.Source.Model.DataStruct;
using Common.Source.Model.Exceptions;
using Common.Source.Model.Interface;
using Common.Source.Service.Encode.QRCode.Metadata;

namespace Common.Source.Service.Encode.QRCode.Encoder
{
    public abstract class QRCodeEncoder : IFreezable
    {
        private static readonly int[,] QRCodeCapacityTable;

        private static readonly ECCodeGroup[,] ECCodeGroupTable;

        public int Capacity { get; private set; }

        public int Version
        {
            get;
            set
            {
                ObjectFrozenException.ThrowIf(this);
                field = value;
            }
        }

        public ECCodeLevel ECCodeLevel
        {
            get;
            set
            {
                ObjectFrozenException.ThrowIf(this);
                field = value;
            }
        }

        public ECCodeGroup ECCodeGroup { get; private set; }

        public abstract EncodeMode EncodeMode { get; }

        protected abstract int BitsOfContent { get; }

        public bool IsFrozen { get; private set; }

        protected abstract BitSet BinaryEncode(ReadOnlySpan<byte> content);

        protected abstract int GetUsedBitCount(int length);

        protected abstract int[,] GetCapacityTable();

        public int GetMaxCapacity()
        {
            return GetCapacityTable()[Version - 1, ECCodeLevel.ToInt()];
        }

        public void SetOptimalVersion(ReadOnlySpan<byte> content, int value = 0)
        {
            ObjectFrozenException.ThrowIf(this);
            int Level = ECCodeLevel.ToInt();
            int[,] CapacityTable = GetCapacityTable();
            int Neutral = CapacityTable.BinarySearch(0, CapacityTable.GetLength(0), (sender, index) => CapacityTable[index, Level], content.Length).GetInsert();
            Version = Math.Clamp(Neutral + 1, value, 40);
        }

        public void Freeze()
        {
            ObjectFrozenException.ThrowIf(this);
            Capacity = QRCodeCapacityTable[Version - 1, ECCodeLevel.ToInt()];
            ECCodeGroup = ECCodeGroupTable[Version - 1, ECCodeLevel.ToInt()];
            IsFrozen = true;
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
                return GaloisField.RSEncode(SubData, ECCodeGroup.ECCodePerBytes);
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

        private BitSet ContentEncode(ReadOnlySpan<byte> content)
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

        private byte[] Interlock(byte[][] content, byte[][] eccode)
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

        static QRCodeEncoder()
        {
            QRCodeCapacityTable = new int[40, 4]
            {
                {19,    16,     13,     9},
                {34,    28,     22,     16},
                {55,    44,     34,     26},
                {80,    64,     48,     36},
                {108,   86,     62,     46},
                {136,   108,    76,     60},
                {156,   124,    88,     66},
                {194,   154,    110,    86},
                {232,   182,    132,    100},
                {274,   216,    154,    122},
                {324,   254,    180,    140},
                {370,   290,    206,    158},
                {428,   334,    244,    180},
                {461,   365,    261,    197},
                {523,   415,    295,    223},
                {589,   453,    325,    253},
                {647,   507,    367,    283},
                {721,   563,    397,    313},
                {795,   627,    445,    341},
                {861,   669,    485,    385},
                {932,   714,    512,    406},
                {1006,  782,    568,    442},
                {1094,  860,    614,    464},
                {1174,  914,    664,    514},
                {1276,  1000,   718,    538},
                {1370,  1062,   754,    596},
                {1468,  1128,   808,    628},
                {1531,  1193,   871,    661},
                {1631,  1267,   911,    701},
                {1735,  1373,   985,    745},
                {1843,  1455,   1033,   793},
                {1955,  1541,   1115,   845},
                {2071,  1631,   1171,   901},
                {2191,  1725,   1231,   961},
                {2306,  1812,   1286,   986},
                {2434,  1914,   1354,   1054},
                {2566,  1992,   1426,   1096},
                {2702,  2102,   1502,   1142},
                {2812,  2216,   1582,   1222},
                {2956,  2334,   1666,   1276},
            };
            ECCodeGroupTable = new ECCodeGroup[,]
            {
                {
                    new ECCodeGroup(7,      1,      19,     0,      0),
                    new ECCodeGroup(10,     1,      16,     0,      0),
                    new ECCodeGroup(13,     1,      13,     0,      0),
                    new ECCodeGroup(17,     1,      9,      0,      0)
                },
                {
                    new ECCodeGroup(10,     1,      34,     0,      0),
                    new ECCodeGroup(16,     1,      28,     0,      0),
                    new ECCodeGroup(22,     1,      22,     0,      0),
                    new ECCodeGroup(28,     1,      16,     0,      0)
                },
                {
                    new ECCodeGroup(15,     1,      55,     0,      0),
                    new ECCodeGroup(26,     1,      44,     0,      0),
                    new ECCodeGroup(18,     2,      17,     0,      0),
                    new ECCodeGroup(22,     2,      13,     0,      0)
                },
                {
                    new ECCodeGroup(20,     1,      80,     0,      0),
                    new ECCodeGroup(18,     2,      32,     0,      0),
                    new ECCodeGroup(26,     2,      24,     0,      0),
                    new ECCodeGroup(16,     4,      9,      0,      0)
                },
                {
                    new ECCodeGroup(26,     1,      108,    0,      0),
                    new ECCodeGroup(24,     2,      43,     0,      0),
                    new ECCodeGroup(18,     2,      15,     2,      16),
                    new ECCodeGroup(22,     2,      11,     2,      12)
                },
                {
                    new ECCodeGroup(18,     2,      68,     0,      0),
                    new ECCodeGroup(16,     4,      27,     0,      0),
                    new ECCodeGroup(24,     4,      19,     0,      0),
                    new ECCodeGroup(28,     4,      15,     0,      0)
                },
                {
                    new ECCodeGroup(20,     2,      78,     0,      0),
                    new ECCodeGroup(18,     4,      31,     0,      0),
                    new ECCodeGroup(18,     2,      14,     4,      15),
                    new ECCodeGroup(26,     4,      13,     1,      14)
                },
                {
                    new ECCodeGroup(24,     2,      97,     0,      0),
                    new ECCodeGroup(22,     2,      38,     2,      39),
                    new ECCodeGroup(22,     4,      18,     2,      19),
                    new ECCodeGroup(26,     4,      14,     2,      15)
                },
                {
                    new ECCodeGroup(30,     2,      116,    0,      0),
                    new ECCodeGroup(22,     3,      36,     2,      37),
                    new ECCodeGroup(20,     4,      16,     4,      17),
                    new ECCodeGroup(24,     4,      12,     4,      13)
                },
                {
                    new ECCodeGroup(18,     2,      68,     2,      69),
                    new ECCodeGroup(26,     4,      43,     1,      44),
                    new ECCodeGroup(24,     6,      19,     2,      20),
                    new ECCodeGroup(28,     6,      15,     2,      16)
                },
                {
                    new ECCodeGroup(20,     4,      81,     0,      0),
                    new ECCodeGroup(30,     1,      50,     4,      51),
                    new ECCodeGroup(28,     4,      22,     4,      23),
                    new ECCodeGroup(24,     3,      12,     8,      13)
                },
                {
                    new ECCodeGroup(24,     2,      92,     2,      93),
                    new ECCodeGroup(22,     6,      36,     2,      37),
                    new ECCodeGroup(26,     4,      20,     6,      21),
                    new ECCodeGroup(28,     7,      14,     4,      15)
                },
                {
                    new ECCodeGroup(26,     4,      107,    0,      0),
                    new ECCodeGroup(22,     8,      37,     1,      38),
                    new ECCodeGroup(24,     8,      20,     4,      21),
                    new ECCodeGroup(22,     12,     11,     4,      12)
                },
                {
                    new ECCodeGroup(30,     3,      115,    1,      116),
                    new ECCodeGroup(24,     4,      40,     5,      41),
                    new ECCodeGroup(20,     11,     16,     5,      17),
                    new ECCodeGroup(24,     11,     12,     5,      13)
                },
                {
                    new ECCodeGroup(22,     5,      87,     1,      88),
                    new ECCodeGroup(24,     5,      41,     5,      42),
                    new ECCodeGroup(30,     5,      24,     7,      25),
                    new ECCodeGroup(24,     11,     12,     7,      13)
                },
                {
                    new ECCodeGroup(24,     5,      98,     1,      99),
                    new ECCodeGroup(28,     7,      45,     3,      46),
                    new ECCodeGroup(24,     15,     19,     2,      20),
                    new ECCodeGroup(30,     3,      15,     13,     16)
                },
                {
                    new ECCodeGroup(28,     1,      107,    5,      108),
                    new ECCodeGroup(28,     10,     46,     1,      47),
                    new ECCodeGroup(28,     1,      22,     15,     23),
                    new ECCodeGroup(28,     2,      14,     17,     15)
                },
                {
                    new ECCodeGroup(30,     5,      120,    1,      121),
                    new ECCodeGroup(26,     9,      43,     4,      44),
                    new ECCodeGroup(28,     17,     22,     1,      23),
                    new ECCodeGroup(28,     2,      14,     19,     15)
                },
                {
                    new ECCodeGroup(28,     3,      113,    4,      114),
                    new ECCodeGroup(26,     3,      44,     11,     45),
                    new ECCodeGroup(26,     17,     21,     4,      22),
                    new ECCodeGroup(26,     9,      13,     16,     14)
                },
                {
                    new ECCodeGroup(28,     3,      107,    5,      108),
                    new ECCodeGroup(26,     3,      41,     13,     42),
                    new ECCodeGroup(30,     15,     24,     5,      25),
                    new ECCodeGroup(28,     15,     15,     10,     16)
                },
                {
                    new ECCodeGroup(28,     4,      116,    4,      117),
                    new ECCodeGroup(26,     17,     42,     0,      0),
                    new ECCodeGroup(28,     17,     22,     6,      23),
                    new ECCodeGroup(30,     19,     16,     6,      17)
                },
                {
                    new ECCodeGroup(28,     2,      111,    7,      112),
                    new ECCodeGroup(28,     17,     46,     0,      0),
                    new ECCodeGroup(30,     7,      24,     16,     25),
                    new ECCodeGroup(24,     34,     13,     0,      0)
                },
                {
                    new ECCodeGroup(30,     4,      121,    5,      122),
                    new ECCodeGroup(28,     4,      47,     14,     48),
                    new ECCodeGroup(30,     11,     24,     14,     25),
                    new ECCodeGroup(30,     16,     15,     14,     16)
                },
                {
                    new ECCodeGroup(30,     6,      117,    4,      118),
                    new ECCodeGroup(28,     6,      45,     14,     46),
                    new ECCodeGroup(30,     11,     24,     16,     25),
                    new ECCodeGroup(30,     30,     16,     2,      17)
                },
                {
                    new ECCodeGroup(26,     8,      106,    4,      107),
                    new ECCodeGroup(28,     8,      47,     13,     48),
                    new ECCodeGroup(30,     7,      24,     22,     25),
                    new ECCodeGroup(30,     22,     15,     13,     16)
                },
                {
                    new ECCodeGroup(28,     10,     114,    2,      115),
                    new ECCodeGroup(28,     19,     46,     4,      47),
                    new ECCodeGroup(28,     28,     22,     6,      23),
                    new ECCodeGroup(30,     33,     16,     4,      17)
                },
                {
                    new ECCodeGroup(30,     8,      122,    4,      123),
                    new ECCodeGroup(28,     22,     45,     3,      46),
                    new ECCodeGroup(30,     8,      23,     26,     24),
                    new ECCodeGroup(30,     12,     15,     28,     16)
                },
                {
                    new ECCodeGroup(30,     3,      117,    10,     118),
                    new ECCodeGroup(28,     3,      45,     23,     46),
                    new ECCodeGroup(30,     4,      24,     31,     25),
                    new ECCodeGroup(30,     11,     15,     31,     16)
                },
                {
                    new ECCodeGroup(30,     7,      116,    7,      117),
                    new ECCodeGroup(28,     21,     45,     7,      46),
                    new ECCodeGroup(30,     1,      23,     37,     24),
                    new ECCodeGroup(30,     19,     15,     26,     16)
                },
                {
                    new ECCodeGroup(30,     5,      115,    10,     116),
                    new ECCodeGroup(28,     19,     47,     10,     48),
                    new ECCodeGroup(30,     15,     24,     25,     25),
                    new ECCodeGroup(30,     23,     15,     25,     16)
                },
                {
                    new ECCodeGroup(30,     13,     115,    3,      116),
                    new ECCodeGroup(28,     2,      46,     29,     47),
                    new ECCodeGroup(30,     42,     24,     1,      25),
                    new ECCodeGroup(30,     23,     15,     28,     16)
                },
                {
                    new ECCodeGroup(30,     17,     115,    0,      0),
                    new ECCodeGroup(28,     10,     46,     23,     47),
                    new ECCodeGroup(30,     10,     24,     35,     25),
                    new ECCodeGroup(30,     19,     15,     35,     16)
                },
                {
                    new ECCodeGroup(30,     17,     115,    1,      116),
                    new ECCodeGroup(28,     14,     46,     21,     47),
                    new ECCodeGroup(30,     29,     24,     19,     25),
                    new ECCodeGroup(30,     11,     15,     46,     16)
                },
                {
                    new ECCodeGroup(30,     13,     115,    6,      116),
                    new ECCodeGroup(28,     14,     46,     23,     47),
                    new ECCodeGroup(30,     44,     24,     7,      25),
                    new ECCodeGroup(30,     59,     16,     1,      17)
                },
                {
                    new ECCodeGroup(30,     12,     121,    7,      122),
                    new ECCodeGroup(28,     12,     47,     26,     48),
                    new ECCodeGroup(30,     39,     24,     14,     25),
                    new ECCodeGroup(30,     22,     15,     41,     16)
                },
                {
                    new ECCodeGroup(30,     6,      121,    14,     122),
                    new ECCodeGroup(28,     6,      47,     34,     48),
                    new ECCodeGroup(30,     46,     24,     10,     25),
                    new ECCodeGroup(30,     2,      15,     64,     16)
                },
                {
                    new ECCodeGroup(30,     17,     122,    4,      123),
                    new ECCodeGroup(28,     29,     46,     14,     47),
                    new ECCodeGroup(30,     49,     24,     10,     25),
                    new ECCodeGroup(30,     24,     15,     46,     16)
                },
                {
                    new ECCodeGroup(30,     4,      122,    18,     123),
                    new ECCodeGroup(28,     13,     46,     32,     47),
                    new ECCodeGroup(30,     48,     24,     14,     25),
                    new ECCodeGroup(30,     42,     15,     32,     16)
                },
                {
                    new ECCodeGroup(30,     20,     117,    4,      118),
                    new ECCodeGroup(28,     40,     47,     7,      48),
                    new ECCodeGroup(30,     43,     24,     22,     25),
                    new ECCodeGroup(30,     10,     15,     67,     16)
                },
                {
                    new ECCodeGroup(30,     19,     118,    6,      119),
                    new ECCodeGroup(28,     18,     47,     31,     48),
                    new ECCodeGroup(30,     34,     24,     34,     25),
                    new ECCodeGroup(30,     20,     15,     61,     16)
                }
            };
        }
    }
}