using Common.Source.Extension;
using Common.Source.Model.DataStruct.Disk.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Model.DataStruct.Disk
{
    public readonly struct DiskSize : IEquatable<DiskSize>, IComparable<DiskSize>
    {
        public readonly long Bytes;

        public DiskSize(long bytes)
        {
            Bytes = bytes;
        }

        public static DiskSize Create(DataSize dataSize, long value = 1)
        {
            return new DiskSize(dataSize.GetBytes(value));
        }

        public static DiskSize Create(DataSize dataSize, double value, MidpointRounding mode = default)
        {
            return new DiskSize(dataSize.GetBytes(value, mode));
        }

        public double GetSize(DataSize dataSize)
        {
            return dataSize.GetSize(Bytes);
        }

        public DataSize GetDataSize()
        {
            return DataSizeExtension.GetDataSize(Bytes);
        }

        public static DiskSize operator +(DiskSize left, DiskSize right)
        {
            return new DiskSize(left.Bytes + right.Bytes);
        }

        public static DiskSize operator -(DiskSize left, DiskSize right)
        {
            return new DiskSize(left.Bytes - right.Bytes);
        }

        public static DiskSize operator *(DiskSize left, long right)
        {
            return new DiskSize(left.Bytes * right);
        }

        public static DiskSize operator *(long left, DiskSize right)
        {
            return new DiskSize(left * right.Bytes);
        }

        public static bool operator >(DiskSize left, DiskSize right)
        {
            return left.Bytes > right.Bytes;
        }

        public static bool operator >=(DiskSize left, DiskSize right)
        {
            return left.Bytes >= right.Bytes;
        }

        public static bool operator <(DiskSize left, DiskSize right)
        {
            return left.Bytes < right.Bytes;
        }

        public static bool operator <=(DiskSize left, DiskSize right)
        {
            return left.Bytes <= right.Bytes;
        }

        public static bool operator ==(DiskSize left, DiskSize right)
        {
            return left.Bytes == right.Bytes;
        }

        public static bool operator !=(DiskSize left, DiskSize right)
        {
            return left.Bytes != right.Bytes;
        }

        public int CompareTo(DiskSize other)
        {
            return Bytes.CompareTo(other.Bytes);
        }

        public bool Equals(DiskSize other)
        {
            return Bytes == other.Bytes;
        }

        public override bool Equals([NotNullWhen(true)] object? sender)
        {
            return sender is DiskSize Other && Equals(Other);
        }

        public override int GetHashCode() => Bytes.GetHashCode();

        public static explicit operator long(DiskSize diskSize)
        {
            return diskSize.Bytes;
        }

        public static explicit operator DiskSize(long bytes)
        {
            return new DiskSize(bytes);
        }

        public override string ToString()
        {
            return $"{GetSize(GetDataSize().OutSelf(out DataSize Current)):0.##}\x20{Current}";
        }
    }
}