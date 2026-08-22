using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UIDesign.Source.Factory.NotifyPropertyChanged
{
    public interface INotifyPropertyChangedFactory : INotifyPropertyChanged
    {
        void OnPropertyChanged([CallerMemberName] string? propertyName = default);

        void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = default);

        void SetField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = default);
    }
}