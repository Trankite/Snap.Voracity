namespace UIDesign.Source.Factory.NotifyPropertyChanged
{
    public interface INotifyPropertyChangedDelayedFactory : INotifyPropertyChangedFactory
    {
        bool IsNotifyPropertyChanged { get; set; }
    }
}