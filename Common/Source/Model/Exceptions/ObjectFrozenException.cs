using Common.Source.Model.Interface;
using System.Diagnostics;

namespace Common.Source.Model.Exceptions
{
    public class ObjectFrozenException : InvalidOperationException
    {
        public ObjectFrozenException() { }

        public ObjectFrozenException(string? message) : base(message) { }

        public ObjectFrozenException(string? message, Exception? innerException) : base(message, innerException) { }

        [StackTraceHidden]
        public static void ThrowIf(IFreezable freezable)
        {
            if (freezable.IsFrozen)
            {
                throw new ObjectFrozenException($"The object of type '{freezable.GetType().FullName}' is frozen and cannot be modified.");
            }
        }
    }
}