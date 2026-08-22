namespace Common.Source.Model.StarRail.Character.Attribute
{
    public interface ICharacterAttributeModel
    {
        double Attack { get; set; }

        double AttackBase { get; set; }

        double Health { get; set; }

        double HealthBase { get; set; }

        double Defense { get; set; }

        double DefenseBase { get; set; }

        double Speed { get; set; }

        double SpeedBase { get; set; }

        double CriticalHitRate { get; set; }

        double CriticalHitDamage { get; set; }

        double ElementIncrease { get; set; }

        double DefenseDecrease { get; set; }

        double MagicalDecrease { get; set; }

        double MagicalIncrease { get; set; }

        double BreakEffect { get; set; }

        double BreakEfficiency { get; set; }

        double BreakIncrease { get; set; }

        double ElationIncrease { get; set; }

        double ToughDecline { get; set; }

        double EffectHitRate { get; set; }

        double EffectMagical { get; set; }

        double HealingBoost { get; set; }

        double ElationBonus { get; set; }

        double ManaReplenish { get; set; }

        double MaximumEnergy { get; set; }

        double CharacterLevel { get; set; }

        double MonsterLevel { get; set; }

        double MonsterCount { get; set; }

        double DamageDecrease { get; set; }

        double DamageIncrease { get; set; }

        double Toughness { get; set; }
    }
}