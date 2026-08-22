using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UIDesign.Source.Factory.NotifyPropertyChanged
{
    public abstract class NotifyPropertyChangedFactory : INotifyPropertyChangedFactory
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = default)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        public void SetField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = default)
        {
            if (predicate(value)) SetField(ref field, value, propertyName);
        }
    }
}