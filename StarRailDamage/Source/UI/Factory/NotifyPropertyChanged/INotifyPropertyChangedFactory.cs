using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StarRailDamage.Source.UI.Factory.NotifyPropertyChanged
{
    public interface INotifyPropertyChangedFactory : INotifyPropertyChanged
    {
        void OnPropertyChanged([CallerMemberName] string? propertyName = default);

        bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = default);

        bool SetField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = default);

        bool SetNotifyField<T>(ref T field, T value, [CallerMemberName] string? propertyName = default) where T : INotifyPropertyChanged;

        bool SetNotifyField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = default) where T : INotifyPropertyChanged;
    }
}