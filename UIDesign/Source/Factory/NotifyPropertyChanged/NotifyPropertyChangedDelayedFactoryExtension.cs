using System.Diagnostics;

namespace UIDesign.Source.Factory.NotifyPropertyChanged
{
    public static class NotifyPropertyChangedDelayedFactoryExtension
    {
        [DebuggerStepThrough]
        public static void ForbidNotify<T>(this INotifyPropertyChangedDelayedFactory factory)
        {
            factory.IsNotifyPropertyChanged = false;
        }

        [DebuggerStepThrough]
        public static void EnableNotify<T>(this INotifyPropertyChangedDelayedFactory factory)
        {
            factory.IsNotifyPropertyChanged = true;
        }
    }
}