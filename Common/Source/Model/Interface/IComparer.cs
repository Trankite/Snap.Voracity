namespace Common.Source.Model.Interface
{
    public interface IComparer<TSender, TOther> : IComparer<TSender> where TOther : allows ref struct
    {
        int Compare(TSender? sender, TOther? other);
    }
}