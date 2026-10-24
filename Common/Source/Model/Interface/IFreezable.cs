namespace Common.Source.Model.Interface
{
    public interface IFreezable
    {
        bool IsFrozen { get; }

        void Freeze();
    }
}