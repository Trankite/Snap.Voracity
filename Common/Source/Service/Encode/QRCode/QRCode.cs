using Common.Source.Extension;
using Common.Source.Model.DataStruct;
using Common.Source.Model.DataStruct.Point;
using Common.Source.Service.Encode.QRCode.Encoder;
using Common.Source.Service.Encode.QRCode.Metadata;
using System.Reflection;
using System.Text;

namespace Common.Source.Service.Encode.QRCode
{
    public sealed class QRCode
    {
        private static readonly int[][] AlignmentPointTable;

        private static readonly LocalPoint[,] FormatPointTable;

        private readonly QRCodePoint[,] Table;

        private readonly QRCodeEncoder Encoder;

        public MaskType MaskType { get; private set; }

        public QRCodePoint this[int x, int y] => Table[x, y];

        public int Size => Encoder.Size;

        public int Version => Encoder.Version;

        public int Capacity => Encoder.GetDataCapacity();

        public EncodeMode EncodeMode => Encoder.EncodeMode;

        public ECCodeLevel ECCodeLevel => Encoder.ECCodeLevel;

        public ECCodeGroup ECCodeGroup => Encoder.ECCodeGroup;

        private QRCode(QRCodeEncoder encoder, MaskType maskType)
        {
            Encoder = encoder;
            Table = new QRCodePoint[Size, Size];
            MaskType = maskType;
        }

        public static QRCode Create(string content, QRCodeOptions? options = default, Encoding? encoding = default)
        {
            encoding ??= Encoding.UTF8;
            return Create(encoding.GetBytes(content), options);
        }

        public static QRCode Create(ReadOnlySpan<byte> content, QRCodeOptions? options = default)
        {
            options ??= new QRCodeOptions();
            EncodeMode mode = options.EncodeMode.GetAutoEncodeMode(content);
            QRCodeEncoder encoder = mode.CreateEncoder();
            encoder.ECCodeLevel = options.ECCodeLevel;
            encoder.SetOptimalVersion(content, options.Version);
            encoder.Freeze();
            QRCode qrcode = new(encoder, options.MaskType);
            qrcode.Initialize(content.SplitAt(encoder.GetDataCapacity()).Head);
            return qrcode;
        }

        private void Initialize(ReadOnlySpan<byte> content)
        {
            SetFixedInformation();
            SetFormatInformation();
            SetVersionInformation();
            SetFlags(content.Length);
            SetContent(content);
            SetOptimalMaskType();
            SetMaskInformation();
        }

        private void SetFixedInformation()
        {
            SetFixedPoint();
            SetPositionPattern();
            SetAlignmentPattern();
            SetTimingPattern();
        }

        private void SetFixedPoint()
        {
            SetBitType(GetAbsolutePoint(8, -8), true, QRCodePointType.FixedPoint);
        }

        private void SetPositionPattern()
        {
            byte[,] pattern = new byte[9, 9];
            byte[] template = [0, 1, 0, 1, 1];
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    pattern[x, y] = template[4 - Math.Max(Math.Abs(x - 4), Math.Abs(y - 4))];
                }
            }
            CopyTable(pattern, -1, -1);
            CopyTable(pattern, -1, Size - 8);
            CopyTable(pattern, Size - 8, -1);
        }

        private void SetAlignmentPattern()
        {
            if (Version > 1)
            {
                byte[,] pattern = new byte[5, 5]
                {
                    { 1, 1, 1, 1, 1 },
                    { 1, 0, 0, 0, 1 },
                    { 1, 0, 1, 0, 1 },
                    { 1, 0, 0, 0, 1 },
                    { 1, 1, 1, 1, 1 }
                };
                int[] points = AlignmentPointTable[Version - 2];
                for (int x = 1; x < points.Length - 1; x++)
                {
                    CopyTable(pattern, points[x] - 2, points[0] - 2);
                }
                for (int y = 1; y < points.Length - 1; y++)
                {
                    CopyTable(pattern, points[0] - 2, points[y] - 2);
                }
                for (int x = 1; x < points.Length; x++)
                {
                    for (int y = 1; y < points.Length; y++)
                    {
                        CopyTable(pattern, points[x] - 2, points[y] - 2);
                    }
                }
            }
        }

        private void SetTimingPattern()
        {
            bool hasBit = true;
            for (int x = 8; x < Size - 8; x++)
            {
                if (Table[x, 6].Type == QRCodePointType.Unused)
                {
                    SetBitType(new LocalPoint(x, 6), hasBit, QRCodePointType.Timing);
                }
                hasBit = !hasBit;
            }
            hasBit = true;
            for (int y = 8; y < Size - 8; y++)
            {
                if (Table[6, y].Type == QRCodePointType.Unused)
                {
                    SetBitType(new LocalPoint(6, y), hasBit, QRCodePointType.Timing);
                }
                hasBit = !hasBit;
            }
        }

        private void SetFormatInformation()
        {
            int formatBinary = (((int)ECCodeLevel ^ 1) << 13) | (((int)MaskType - 1) << 10);
            formatBinary = (formatBinary | GaloisField.PolynomMod(formatBinary, 0b10100110111)) ^ 0b101010000010010;
            for (int i = 0; i < 15; i++)
            {
                bool hasBit = (formatBinary & (1 << i)) != 0;
                for (int k = 0; k < 2; k++)
                {
                    SetBitType(GetAbsolutePoint(FormatPointTable[k, i]), hasBit, QRCodePointType.Format);
                }
            }
        }

        private void SetVersionInformation()
        {
            if (Version >= 7)
            {
                int bits = Version << 12;
                bits |= GaloisField.PolynomMod(bits, 0b1111100100101);
                for (int i = 0; i < 6; i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        bool hasBit = (bits & (1 << (i * 3 + k))) != 0;
                        SetBitType(GetAbsolutePoint(-11 + k, i), hasBit, QRCodePointType.Version);
                        SetBitType(GetAbsolutePoint(i, -11 + k), hasBit, QRCodePointType.Version);
                    }
                }
            }
        }

        private void SetFlags(int length)
        {
            int contentTotalBits = Encoder.GetTotalBitCount(length);
            int contentPaddingTotalBits = Encoder.Capacity * 8 - contentTotalBits;
            int eccodeTotalBits = (ECCodeGroup.BlocksInGroup1 + ECCodeGroup.BlocksInGroup2) * ECCodeGroup.ECCodePerBytes * 8;
            using IEnumerator<SpacePoint> pointArray = GetPoints(1 << (int)QRCodePointType.Unused).GetEnumerator();
            void SetBitTypeFlag(int length, QRCodePointType type)
            {
                for (int i = length; i > 0 && pointArray.MoveNext(); i--)
                {
                    Table[pointArray.Current.X, pointArray.Current.Y].Type = type;
                }
            }
            SetBitTypeFlag(contentTotalBits, QRCodePointType.Content);
            SetBitTypeFlag(contentPaddingTotalBits, QRCodePointType.ContentPadding);
            SetBitTypeFlag(eccodeTotalBits, QRCodePointType.ECCode);
            while (pointArray.MoveNext())
            {
                Table[pointArray.Current.X, pointArray.Current.Y].Type = QRCodePointType.Padding;
            }
        }

        private void SetContent(ReadOnlySpan<byte> data)
        {
            BitSet bits = BitSet.Create(Encoder.Encode(data));
            int flags = EnumExtension.GetFlags(QRCodePointType.Unused, QRCodePointType.Content, QRCodePointType.ContentPadding, QRCodePointType.ECCode);
            using IEnumerator<SpacePoint> enumerator = GetPoints(flags).GetEnumerator();
            while (enumerator.TryMoveNext(out SpacePoint point) && point.Z <= bits.Count)
            {
                Table[point.X, point.Y].HasBit = bits[point.Z];
            }
        }

        public IEnumerable<SpacePoint> GetPoints(int flags)
        {
            int index = default;
            int magicSize = Size * (Size - 1);
            int enchantment = (Size - 7) / 2;
            for (int i = 0; i < magicSize; i++)
            {
                int position1 = Math.DivRem(i, Size * 2, out int magic);
                int position2 = Math.DivRem(magic, 2, out magic);
                int x = Size - 1 - (position1 < enchantment ? position1 * 2 + magic : position1 * 2 + magic + 1);
                int y = Size - 1 - (position1 % 2 == 0 ? position2 : Size - 1 - position2);
                if (((1 << (int)Table[x, y].Type) & flags) != 0)
                {
                    yield return new SpacePoint(x, y, index++);
                }
            }
        }

        private void CopyTable(byte[,] table, int destinationX, int destinationY)
        {
            int count = table.GetLength(0);
            for (int x = destinationX; x < destinationX + count; x++)
            {
                if (x < 0 || x >= Size)
                {
                    continue;
                }
                for (int y = destinationY; y < destinationY + count; y++)
                {
                    if (y >= 0 && y < Size)
                    {
                        SetBitType(new LocalPoint(x, y), table[x - destinationX, y - destinationY] != 0, QRCodePointType.Fixed);
                    }
                }
            }
        }

        private LocalPoint GetAbsolutePoint(int x, int y)
        {
            return new LocalPoint(x < 0 ? x + Size : x, y < 0 ? y + Size : y);
        }

        private LocalPoint GetAbsolutePoint(LocalPoint point)
        {
            return GetAbsolutePoint(point.X, point.Y);
        }

        private void SetBitType(in LocalPoint point, bool hasBit, QRCodePointType type)
        {
            Table[point.X, point.Y].SetValue(hasBit, type);
        }

        private void SetOptimalMaskType()
        {
            if (MaskType == MaskType.Optimal)
            {
                const int MaskCount = 8;
                MaskType optimal = default;
                int maxScore = int.MinValue;
                for (int i = 1; i <= MaskCount; i++)
                {
                    MaskType = (MaskType)i;
                    SetMaskInformation();
                    SetFormatInformation();
                    int score = GetMaskScore();
                    if (score >= maxScore)
                    {
                        maxScore = score;
                        optimal = MaskType;
                    }
                    SetMaskInformation();
                }
                MaskType = optimal;
                SetFormatInformation();
            }
        }

        private void SetMaskInformation()
        {
            Func<int, int, bool> method = MaskType.GetMethod();
            int flags = EnumExtension.GetFlags(QRCodePointType.Unused, QRCodePointType.Content, QRCodePointType.ContentPadding, QRCodePointType.ECCode, QRCodePointType.Padding);
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (((1 << (int)Table[x, y].Type) & flags) != 0)
                    {
                        Table[x, y].HasBit = method(x, y) != Table[x, y].HasBit;
                    }
                }
            }
        }

        private int GetMaskScore()
        {
            int score = default;
            score -= GetSameColorScore();
            score -= GetSameBlockScore();
            score -= GetAsPatternScore();
            score -= GetUnbalanceScore();
            return score;
        }

        private int GetSameColorScore()
        {
            int score = default;
            const int MinSame = 5;
            const int DefaultScore = 3;
            for (int x = 0; x < Size; x++)
            {
                int total = 1;
                bool current = Table[x, 0].HasBit;
                for (int y = 1; y < Size; y++)
                {
                    if (Table[x, y].HasBit == current)
                    {
                        total++;
                    }
                    else
                    {
                        if (total >= MinSame)
                        {
                            score += DefaultScore + (total - MinSame);
                        }
                        current = Table[x, y].HasBit;
                        total = 1;
                    }
                }
                if (total >= MinSame)
                {
                    score += DefaultScore + (total - MinSame);
                }
            }
            for (int y = 0; y < Size; y++)
            {
                int total = 1;
                bool current = Table[0, y].HasBit;
                for (int x = 1; x < Size; x++)
                {
                    if (Table[x, y].HasBit == current)
                    {
                        total++;
                    }
                    else
                    {
                        if (total >= MinSame)
                        {
                            score += DefaultScore + (total - MinSame);
                        }
                        current = Table[x, y].HasBit;
                        total = 1;
                    }
                }
                if (total >= MinSame)
                {
                    score += DefaultScore + (total - MinSame);
                }
            }
            return score;
        }

        private int GetSameBlockScore()
        {
            int score = default;
            const int DefaultScore = 3;
            for (int x = Size - 1; x > 0; x--)
            {
                for (int y = Size - 1; y > 0; y--)
                {
                    bool current = Table[x, y].HasBit;
                    if (current == Table[x - 1, y].HasBit && current == Table[x, y - 1].HasBit && current == Table[x - 1, y - 1].HasBit)
                    {
                        score += DefaultScore;
                    }
                }
            }
            return score;
        }

        private int GetAsPatternScore()
        {
            int score = default;
            const int DefaultScore = 40;
            const int Pattern = 0b1011101;
            const int HighPattern = Pattern << 4;
            const int PatternMaxLength = 11;
            const int PatternPadding = 4;
            for (int x = 0; x < Size; x++)
            {
                BitStack<int> bitStack = default;
                for (int y = 0; y < PatternMaxLength; y++)
                {
                    bitStack.Push(Table[x, y] & 1);
                }
                for (int y = PatternMaxLength; y < Size; y++)
                {
                    bitStack.Push(Table[x, y] & 1);
                    int current = bitStack.Peek(PatternMaxLength);
                    if (current == Pattern || current == HighPattern && bitStack.Peek(PatternPadding, PatternMaxLength) != 0)
                    {
                        score += DefaultScore;
                    }
                }
            }
            for (int y = 0; y < Size; y++)
            {
                BitStack<int> bitStack = default;
                for (int x = 0; x < PatternMaxLength; x++)
                {
                    bitStack.Push(Table[x, y] & 1);
                }
                for (int x = PatternMaxLength; x < Size; x++)
                {
                    bitStack.Push(Table[x, y] & 1);
                    int current = bitStack.Peek(PatternMaxLength);
                    if (current == Pattern || current == HighPattern && bitStack.Peek(PatternPadding, PatternMaxLength) != 0)
                    {
                        score += DefaultScore;
                    }
                }
            }
            return score;
        }

        private int GetUnbalanceScore()
        {
            int count = default;
            foreach (QRCodePoint current in Table)
            {
                count += current & 1;
            }
            int score = count * 100 / Table.Length - 50;
            return Math.Abs(score / 5 * 10);
        }

        static QRCode()
        {
            FormatPointTable = new LocalPoint[2, 15]
            {
                {
                    new LocalPoint(8, 0),
                    new LocalPoint(8, 1),
                    new LocalPoint(8, 2),
                    new LocalPoint(8, 3),
                    new LocalPoint(8, 4),
                    new LocalPoint(8, 5),
                    new LocalPoint(8, 7),
                    new LocalPoint(8, 8),
                    new LocalPoint(7, 8),
                    new LocalPoint(5, 8),
                    new LocalPoint(4, 8),
                    new LocalPoint(3, 8),
                    new LocalPoint(2, 8),
                    new LocalPoint(1, 8),
                    new LocalPoint(0, 8)
                },
                {
                    new LocalPoint(-1, 8),
                    new LocalPoint(-2, 8),
                    new LocalPoint(-3, 8),
                    new LocalPoint(-4, 8),
                    new LocalPoint(-5, 8),
                    new LocalPoint(-6, 8),
                    new LocalPoint(-7, 8),
                    new LocalPoint(-8, 8),
                    new LocalPoint(8, -7),
                    new LocalPoint(8, -6),
                    new LocalPoint(8, -5),
                    new LocalPoint(8, -4),
                    new LocalPoint(8, -3),
                    new LocalPoint(8, -2),
                    new LocalPoint(8, -1)
                }
            };
            AlignmentPointTable =
            [
                [6,               /* Version 2 */               18],
                [6,               /* Version 3 */               22],
                [6,               /* Version 4 */               26],
                [6,               /* Version 5 */               30],
                [6,               /* Version 6 */               34],
                [6, 22,             /* Version 7 */             38],
                [6, 24,             /* Version 8 */             42],
                [6, 26,             /* Version 9 */             46],
                [6, 28,             /* Version 10 */            50],
                [6, 30,             /* Version 11 */            54],
                [6, 32,             /* Version 12 */            58],
                [6, 34,               /* Version 13 */          62],
                [6, 26, 46,           /* Version 14 */          66],
                [6, 26, 48,           /* Version 15 */          70],
                [6, 26, 50,           /* Version 16 */          74],
                [6, 30, 54,           /* Version 17 */          78],
                [6, 30, 56,           /* Version 18 */          82],
                [6, 30, 58,           /* Version 19 */          86],
                [6, 34, 62,           /* Version 20 */          90],
                [6, 28, 50, 72,        /* Version 21 */         94],
                [6, 26, 50, 74,        /* Version 22 */         98],
                [6, 30, 54, 78,        /* Version 23 */         102],
                [6, 28, 54, 80,        /* Version 24 */         106],
                [6, 32, 58, 84,        /* Version 25 */         110],
                [6, 30, 58, 86,        /* Version 26 */         114],
                [6, 34, 62, 90,        /* Version 27 */         118],
                [6, 26, 50, 74, 98,      /* Version 28 */       122],
                [6, 30, 54, 78, 102,     /* Version 29 */       126],
                [6, 26, 52, 78, 104,     /* Version 30 */       130],
                [6, 30, 56, 82, 108,     /* Version 31 */       134],
                [6, 34, 60, 86, 112,     /* Version 32 */       138],
                [6, 30, 58, 86, 114,     /* Version 33 */       142],
                [6, 34, 62, 90, 118,     /* Version 34 */       146],
                [6, 30, 54, 78, 102, 126,    /* Version 35 */   150],
                [6, 24, 50, 76, 102, 128,    /* Version 36 */   154],
                [6, 28, 54, 80, 106, 132,    /* Version 37 */   158],
                [6, 32, 58, 84, 110, 136,    /* Version 38 */   162],
                [6, 26, 54, 82, 110, 138,    /* Version 39 */   166],
                [6, 30, 58, 86, 114, 142,    /* Version 40 */   170]
            ];
        }
    }
}