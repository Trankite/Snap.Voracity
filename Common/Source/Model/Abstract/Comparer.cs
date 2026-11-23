using Common.Source.Model.Interface;

namespace Common.Source.Model.Abstract
{
    public abstract class Comparer<TSender, TOther> : IComparer<TSender, TOther> where TOther : allows ref struct
    {
        public virtual int Compare(TSender? sender, TSender? other)
        {
            return Comparer<TSender>.Default.Compare(sender, other);
        }

        public abstract int Compare(TSender? sender, TOther? other);
    }
}