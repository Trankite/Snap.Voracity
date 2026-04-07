using StarRailDamage.Source.Core.LocalText.Fixed;
using StarRailDamage.Source.Core.LocalText.Fixed.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.Text;
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

        private static KeyValuePair<string, CharacterAttributeInfo> GetAttribute(CharacterAttribute attribute, BitmapImage icon, TextBinding unit, int digits)
        {
            return KeyValuePair.Create(attribute.ToString(), CharacterAttributeInfo.Create(attribute.ToString(), icon, unit, digits));
        }

        static CharacterAttributeExtension()
        {
            TextBinding LevelUnit = FixedTextManage.Binding(nameof(FixedText.UnitLevel));
            TextBinding PercentUnit = FixedTextManage.Binding(nameof(FixedText.UnitPercent));
            AttributeMap = FrozenDictionary.Create([
                GetAttribute(CharacterAttribute.Attack,                 AttributeImage.Attack,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.AttackBase,             AttributeImage.Attack,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.Health,                 AttributeImage.Health,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.HealthBase,             AttributeImage.Health,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.Defense,                AttributeImage.Defense,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.DefenseBase,            AttributeImage.Defense,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.Speed,                  AttributeImage.Speed,       TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.SpeedBase,              AttributeImage.Speed,       TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.CriticalHitRate,        AttributeImage.Critical,    PercentUnit,            1),
                GetAttribute(CharacterAttribute.CriticalHitDamage,      AttributeImage.Offense,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.ElementIncrease,        AttributeImage.Unknown,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.DefenseDecrease,        AttributeImage.Defense,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.MagicalDecrease,        AttributeImage.Unknown,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.SuperBreakEqual,        AttributeImage.Break,       PercentUnit,            1),
                GetAttribute(CharacterAttribute.BreakEffect,            AttributeImage.Break,       PercentUnit,            1),
                GetAttribute(CharacterAttribute.BreakIncrease,          AttributeImage.Break,       PercentUnit,            1),
                GetAttribute(CharacterAttribute.BreakEfficiency,        AttributeImage.Break,       PercentUnit,            1),
                GetAttribute(CharacterAttribute.ToughDecline,           AttributeImage.Maximum,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.EffectHitRate,          AttributeImage.HitRate,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.EffectMagical,          AttributeImage.Magical,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.HealingBoost,           AttributeImage.Healing,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.HealingCount,           AttributeImage.Maximum,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.ManaReplenish,          AttributeImage.Replenish,   PercentUnit,            1),
                GetAttribute(CharacterAttribute.MaximumEnergy,          AttributeImage.Maximum,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.PersonaLevel,           AttributeImage.Health,      LevelUnit,              0),
                GetAttribute(CharacterAttribute.MonsterLevel,           AttributeImage.Health,      LevelUnit,              0),
                GetAttribute(CharacterAttribute.MonsterCount,           AttributeImage.Unknown,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.ElementMagical,         AttributeImage.Unknown,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.DamageDecrease,         AttributeImage.Defense,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.DamageIncrease,         AttributeImage.Unknown,     PercentUnit,            1),
                GetAttribute(CharacterAttribute.Toughness,              AttributeImage.Maximum,     TextBinding.Default,    0),
                ]);
        }
    }
}