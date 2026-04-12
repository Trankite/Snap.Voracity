using StarRailDamage.Source.UI.Factory.NotifyPropertyChanged;

namespace StarRailDamage.Source.Model.Metadata.Character.Attribute
{
    public class CharacterAttributeModel : NotifyPropertyChangedFactory
    {
        private double _Attack;

        private double _AttackBase;

        private double _Health;

        private double _HealthBase;

        private double _Defense;

        private double _DefenseBase;

        private double _Speed;

        private double _SpeedBase;

        private double _CriticalHitRate;

        private double _CriticalHitDamage;

        private double _ElementIncrease;

        private double _DefenseDecrease;

        private double _MagicalDecrease;

        private double _MagicalIncrease;

        private double _SuperBreakEqual;

        private double _BreakEffect;

        private double _BreakIncrease;

        private double _BreakEfficiency;

        private double _ToughDecline;

        private double _EffectHitRate;

        private double _EffectMagical;

        private double _HealingBoost;

        private double _ElationBonus;

        private double _ManaReplenish;

        private double _MaximumEnergy;

        private double _WonsterLevel;

        private double _MonsterLevel;

        private double _MonsterCount;

        private double _DamageDecrease;

        private double _DamageIncrease;

        private double _Toughness;

        public double Attack
        {
            get => _Attack;
            set => SetField(ref _Attack, value);
        }

        public double AttackBase
        {
            get => _AttackBase;
            set => SetField(ref _AttackBase, value);
        }

        public double Health
        {
            get => _Health;
            set => SetField(ref _Health, value);
        }

        public double HealthBase
        {
            get => _HealthBase;
            set => SetField(ref _HealthBase, value);
        }

        public double Defense
        {
            get => _Defense;
            set => SetField(ref _Defense, value);
        }

        public double DefenseBase
        {
            get => _DefenseBase;
            set => SetField(ref _DefenseBase, value);
        }

        public double Speed
        {
            get => _Speed;
            set => SetField(ref _Speed, value);
        }

        public double SpeedBase
        {
            get => _SpeedBase;
            set => SetField(ref _SpeedBase, value);
        }

        public double CriticalHitRate
        {
            get => _CriticalHitRate;
            set => SetField(ref _CriticalHitRate, value);
        }

        public double CriticalHitDamage
        {
            get => _CriticalHitDamage;
            set => SetField(ref _CriticalHitDamage, value);
        }

        public double ElementIncrease
        {
            get => _ElementIncrease;
            set => SetField(ref _ElementIncrease, value);
        }

        public double DefenseDecrease
        {
            get => _DefenseDecrease;
            set => SetField(ref _DefenseDecrease, value);
        }

        public double MagicalDecrease
        {
            get => _MagicalDecrease;
            set => SetField(ref _MagicalDecrease, value);
        }

        public double MagicalIncrease
        {
            get => _MagicalIncrease;
            set => SetField(ref _MagicalIncrease, value);
        }

        public double SuperBreakEqual
        {
            get => _SuperBreakEqual;
            set => SetField(ref _SuperBreakEqual, value);
        }

        public double BreakEffect
        {
            get => _BreakEffect;
            set => SetField(ref _BreakEffect, value);
        }

        public double BreakIncrease
        {
            get => _BreakIncrease;
            set => SetField(ref _BreakIncrease, value);
        }

        public double BreakEfficiency
        {
            get => _BreakEfficiency;
            set => SetField(ref _BreakEfficiency, value);
        }

        public double ToughDecline
        {
            get => _ToughDecline;
            set => SetField(ref _ToughDecline, value);
        }

        public double EffectHitRate
        {
            get => _EffectHitRate;
            set => SetField(ref _EffectHitRate, value);
        }

        public double EffectMagical
        {
            get => _EffectMagical;
            set => SetField(ref _EffectMagical, value);
        }

        public double HealingBoost
        {
            get => _HealingBoost;
            set => SetField(ref _HealingBoost, value);
        }

        public double ElationBonus
        {
            get => _ElationBonus;
            set => SetField(ref _ElationBonus, value);
        }

        public double ManaReplenish
        {
            get => _ManaReplenish;
            set => SetField(ref _ManaReplenish, value);
        }

        public double MaximumEnergy
        {
            get => _MaximumEnergy;
            set => SetField(ref _MaximumEnergy, value);
        }

        public double WonsterLevel
        {
            get => _WonsterLevel;
            set => SetField(ref _WonsterLevel, value);
        }

        public double MonsterLevel
        {
            get => _MonsterLevel;
            set => SetField(ref _MonsterLevel, value);
        }

        public double MonsterCount
        {
            get => _MonsterCount;
            set => SetField(ref _MonsterCount, value);
        }

        public double DamageDecrease
        {
            get => _DamageDecrease;
            set => SetField(ref _DamageDecrease, value);
        }

        public double DamageIncrease
        {
            get => _DamageIncrease;
            set => SetField(ref _DamageIncrease, value);
        }

        public double Toughness
        {
            get => _Toughness;
            set => SetField(ref _Toughness, value);
        }
    }
}