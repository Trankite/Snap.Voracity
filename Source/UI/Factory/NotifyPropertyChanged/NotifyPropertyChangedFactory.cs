using StarRailDamage.Source.Extension;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StarRailDamage.Source.UI.Factory.NotifyPropertyChanged
{
    public abstract partial class NotifyPropertyChangedFactory : INotifyPropertyChangedFactory
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly Dictionary<string, PropertyChangedEventHandler> Handlers = [];

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            return !Equals(field, value) && true.Configure(OnPropertyChanged, propertyName.Configure(field = value));
        }

        public bool SetField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = null)
        {
            return predicate(value) && SetField(ref field, value, propertyName);
        }

        public bool SetNotifyField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) where T : INotifyPropertyChanged
        {
            if (Equals(field, value)) return false;
            ArgumentNullException.ThrowIfNull(propertyName);
            if (field.IsNotNull())
            {
                field.PropertyChanged -= Handlers.GetValueOrDefault(propertyName);
            }
            if (ObjectExtension.IsNotNull(field = value))
            {
                void PropertyChanged(object? sender, PropertyChangedEventArgs e)
                {
                    OnPropertyChanged($"{propertyName}.{e.PropertyName}");
                }
                Handlers[propertyName] = PropertyChanged;
                field.PropertyChanged += PropertyChanged;
            }
            return true;
        }

        public bool SetNotifyField<T>(ref T field, T value, Predicate<T> predicate, [CallerMemberName] string? propertyName = null) where T : INotifyPropertyChanged
        {
            return predicate(value) && SetNotifyField(ref field, value, propertyName);
        }
    }
}