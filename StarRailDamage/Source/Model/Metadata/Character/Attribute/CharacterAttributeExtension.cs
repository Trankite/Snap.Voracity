using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Model.Metadata.Character.Attribute
{
    public static class CharacterAttributeExtension
    {
        private static readonly FrozenDictionary<string, CharacterAttributeInfo> AttributeMap;

        [DebuggerStepThrough]
        public static CharacterAttributeInfo GetModel(string target)
        {
            return AttributeMap.GetValueOrDefault(target).ThrowIfNull();
        }

        private static KeyValuePair<string, CharacterAttributeInfo> GetAttribute(CharacterAttribute attribute, BitmapImage icon, string unit, int digits)
        {
            return KeyValuePair.Create(attribute.ToString(), CharacterAttributeInfo.Create(attribute.ToString(), icon, unit, digits));
        }

        static CharacterAttributeExtension()
        {
            string UnitLevel = LocalString.ViewPageMockBattleUnitLevel;
            string UnitModulo = LocalString.ViewPageMockBattleUnitModulo;
            AttributeMap = FrozenDictionary.Create([
                GetAttribute(CharacterAttribute.Attack,                 AttributeImage.Attack,      string.Empty,   0),
                GetAttribute(CharacterAttribute.AttackBase,             AttributeImage.Attack,      string.Empty,   0),
                GetAttribute(CharacterAttribute.Health,                 AttributeImage.Health,      string.Empty,   0),
                GetAttribute(CharacterAttribute.HealthBase,             AttributeImage.Health,      string.Empty,   0),
                GetAttribute(CharacterAttribute.Defense,                AttributeImage.Defense,     string.Empty,   0),
                GetAttribute(CharacterAttribute.DefenseBase,            AttributeImage.Defense,     string.Empty,   0),
                GetAttribute(CharacterAttribute.Speed,                  AttributeImage.Speed,       string.Empty,   0),
                GetAttribute(CharacterAttribute.SpeedBase,              AttributeImage.Speed,       string.Empty,   0),
                GetAttribute(CharacterAttribute.CriticalHitRate,        AttributeImage.Critical,    UnitModulo,     1),
                GetAttribute(CharacterAttribute.CriticalHitDamage,      AttributeImage.Offense,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.ElementIncrease,        AttributeImage.Unknown,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.DefenseDecrease,        AttributeImage.Defense,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.MagicalDecrease,        AttributeImage.Unknown,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.BreakEffect,            AttributeImage.Break,       UnitModulo,     1),
                GetAttribute(CharacterAttribute.BreakEfficiency,        AttributeImage.Break,       UnitModulo,     1),
                GetAttribute(CharacterAttribute.BreakIncrease,          AttributeImage.Break,       UnitModulo,     1),
                GetAttribute(CharacterAttribute.ElationIncrease,        AttributeImage.Punchline,   UnitModulo,     1),
                GetAttribute(CharacterAttribute.ToughDecline,           AttributeImage.Maximum,     string.Empty,   0),
                GetAttribute(CharacterAttribute.EffectHitRate,          AttributeImage.HitRate,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.EffectMagical,          AttributeImage.Magical,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.HealingBoost,           AttributeImage.Healing,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.ElationBonus,           AttributeImage.Punchline,   string.Empty,   0),
                GetAttribute(CharacterAttribute.ManaReplenish,          AttributeImage.Replenish,   UnitModulo,     1),
                GetAttribute(CharacterAttribute.MaximumEnergy,          AttributeImage.Maximum,     string.Empty,   0),
                GetAttribute(CharacterAttribute.WonsterLevel,           AttributeImage.Health,      UnitLevel,      0),
                GetAttribute(CharacterAttribute.MonsterLevel,           AttributeImage.Health,      UnitLevel,      0),
                GetAttribute(CharacterAttribute.MonsterCount,           AttributeImage.Unknown,     string.Empty,   0),
                GetAttribute(CharacterAttribute.MagicalIncrease,        AttributeImage.Unknown,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.DamageDecrease,         AttributeImage.Defense,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.DamageIncrease,         AttributeImage.Unknown,     UnitModulo,     1),
                GetAttribute(CharacterAttribute.Toughness,              AttributeImage.Maximum,     string.Empty,   0),
                ]);
        }
    }
}