namespace UIDesign.Source.Factory.NotifyPropertyChanged
{
    public static class NotifyPropertyChangedDelayedFactoryExtension
    {
        public static void ForbidNotify<T>(this INotifyPropertyChangedDelayedFactory factory)
        {
            factory.IsNotifyPropertyChanged = false;
        }

        public static void EnableNotify<T>(this INotifyPropertyChangedDelayedFactory factory)
        {
            factory.IsNotifyPropertyChanged = true;
        }
    }
}