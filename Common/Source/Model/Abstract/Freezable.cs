using Common.Source.Model.Exceptions;
using Common.Source.Model.Interface;

namespace Common.Source.Model.Abstract
{
    public abstract class Freezable : IFreezable
    {
        public bool IsFrozen { get; protected set; }

        public void Freeze()
        {
            ObjectFrozenException.ThrowIfFrozen(this);
            FreezeOverride();
            IsFrozen = true;
        }

        protected void SetValue<T>(ref T source, T value)
        {
            SetValue(this, ref source, value);
        }

        public static void SetValue<T>(IFreezable freezable, ref T source, T value)
        {
            ObjectFrozenException.ThrowIfFrozen(freezable);
            source = value;
        }

        protected abstract void FreezeOverride();
    }
}