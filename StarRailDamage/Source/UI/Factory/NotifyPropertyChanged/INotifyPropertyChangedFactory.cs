using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StarRailDamage.Source.UI.Factory.NotifyPropertyChanged
{
    public interface INotifyPropertyChangedFactory : INotifyPropertyChanged
    {
        void OnPropertyChanged([CallerMemberName] string? propertyName = null);

        bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null);

        bool SetField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = null);

        bool SetNotifyField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) where T : INotifyPropertyChanged;

        bool SetNotifyField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = null) where T : INotifyPropertyChanged;
    }
}