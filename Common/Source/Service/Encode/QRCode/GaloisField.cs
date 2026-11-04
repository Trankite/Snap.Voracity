using System.Numerics;

namespace Common.Source.Service.Encode.QRCode
{
    public readonly struct GaloisField
    {
        private static readonly Dictionary<int, XPolynom> PolynomCache = [];

        private const int Size = 255;

        public readonly static GaloisField One = new(1);

        public readonly static GaloisField Zero = default;

        public readonly static byte[] AlphaTable = new byte[Size + 1];

        public readonly static byte[] ExponentTable = new byte[Size + 1];

        public byte Polynom { get; }

        private GaloisField(int polynom)
        {
            Polynom = (byte)polynom;
        }

        private GaloisField(byte polynom)
        {
            Polynom = polynom;
        }

        private static GaloisField FromExponent(int exponent)
        {
            return new GaloisField(AlphaTable[exponent + 1]);
        }

        public static int PolynomMod(int left, int right)
        {
            int rightLength = GetBitLength(right);
            for (int i = GetBitLength(left) - rightLength - 1; i >= -1; i--)
            {
                if ((left & (1 << (i + rightLength))) != 0)
                {
                    left ^= right << (i + 1);
                }
            }
            return left;
        }

        private static int GetBitLength(int value)
        {
            return 32 - BitOperations.LeadingZeroCount((uint)value);
        }

        public static GaloisField operator +(GaloisField left, GaloisField right)
        {
            return new GaloisField(left.Polynom ^ right.Polynom);
        }

        public static GaloisField operator *(GaloisField left, GaloisField right)
        {
            if (left.Polynom == 0 || right.Polynom == 0) return Zero;
            int exponent = ExponentTable[left.Polynom] + ExponentTable[right.Polynom];
            return FromExponent(exponent >= Size ? exponent - Size : exponent);
        }

        private readonly struct XPolynom
        {
            private GaloisField[] Polynoms { get; }

            public readonly int Count => Polynoms.Length;

            private XPolynom(int count)
            {
                Polynoms = new GaloisField[count];
            }

            public XPolynom(params GaloisField[] polynoms)
            {
                Polynoms = polynoms;
            }

            public readonly ref GaloisField this[int index] => ref Polynoms[index];

            public static XPolynom operator *(XPolynom left, XPolynom right)
            {
                XPolynom xpolynom = new(left.Count + right.Count - 1);
                for (int i = 0; i < left.Count; i++)
                {
                    for (int j = 0; j < right.Count; j++)
                    {
                        xpolynom[i + j] += left[i] * right[j];
                    }
                }
                return xpolynom;
            }
        }

        public static byte[] RSEncode(ReadOnlySpan<byte> content, int count)
        {
            byte[] eccodeBytes = new byte[count];
            Span<byte> eccodeSpan = eccodeBytes.AsSpan();
            if (!PolynomCache.TryGetValue(count, out XPolynom xpolynom))
            {
                xpolynom = new(One, One);
                for (int i = 1; i < count; i++)
                {
                    xpolynom *= new XPolynom(FromExponent(i), One);
                }
                PolynomCache[count] = xpolynom;
            }
            content[..Math.Min(content.Length, eccodeSpan.Length)].CopyTo(eccodeSpan);
            for (int i = 0; i < content.Length; i++)
            {
                GaloisField galoisField = new(eccodeSpan[0]);
                for (int k = 1; k < count; k++)
                {
                    eccodeSpan[k - 1] = (new GaloisField(eccodeSpan[k]) + xpolynom[count - k] * galoisField).Polynom;
                }
                if (i + count < content.Length)
                {
                    eccodeSpan[count - 1] = (new GaloisField(content[i + count]) + xpolynom[0] * galoisField).Polynom;
                }
                else
                {
                    eccodeSpan[count - 1] = (xpolynom[0] * galoisField).Polynom;
                }
            }
            return eccodeBytes;
        }

        public override readonly string ToString()
        {
            return Polynom.ToString();
        }

        static GaloisField()
        {
            AlphaTable[1] = 1;
            for (int i = 2; i < Size + 1; i++)
            {
                int alpha = AlphaTable[i - 1] << 1;
                AlphaTable[i] = (byte)(alpha > Size ? alpha ^ 0b100011101 : alpha);
            }
            ExponentTable[0] = Size;
            for (int i = 1; i < Size + 1; i++)
            {
                ExponentTable[AlphaTable[i]] = (byte)(i - 1);
            }
        }
    }
}