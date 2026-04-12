using StarRailDamage.Source.UI.Factory.NotifyPropertyChanged;

namespace StarRailDamage.Source.Model.Metadata.Character.Damage
{
    public class CharacterDamageModel : NotifyPropertyChangedFactory
    {
        private double _Normal;

        private double _Critical;

        private double _Elation;

        private double _Break;

        private double _SuperBreak;

        private double _Delay;

        public double Normal
        {
            get => _Normal;
            set => SetField(ref _Normal, value);
        }

        public double Critical
        {
            get => _Critical;
            set => SetField(ref _Critical, value);
        }

        public double Elation
        {
            get => _Elation;
            set => SetField(ref _Elation, value);
        }

        public double Break
        {
            get => _Break;
            set => SetField(ref _Break, value);
        }

        public double SuperBreak
        {
            get => _SuperBreak;
            set => SetField(ref _SuperBreak, value);
        }

        public double Delay
        {
            get => _Delay;
            set => SetField(ref _Delay, value);
        }
    }
}