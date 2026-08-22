using Common.Source.Extension;
using System.Runtime.CompilerServices;

namespace UIDesign.Source.Factory.NotifyPropertyChanged
{
    public abstract class NotifyPropertyChangedDelayedFactory : NotifyPropertyChangedFactory, INotifyPropertyChangedDelayedFactory
    {
        private readonly HashSet<string?> ChangedProperties = [];

        public bool IsNotifyPropertyChanged
        {
            get;
            set
            {
                if (field != value && (field = value))
                {
                    ChangedProperties.Foreach(OnPropertyChanged);
                }
            }
        }

        public override void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (IsNotifyPropertyChanged)
            {
                base.SetField(ref field, value, propertyName);
            }
            else
            {
                ChangedProperties.Add(propertyName);
            }
        }
    }
}