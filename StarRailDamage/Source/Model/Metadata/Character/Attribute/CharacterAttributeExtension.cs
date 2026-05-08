using StarRailDamage.Source.Core.LocalText;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.Text;
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

        private static KeyValuePair<string, CharacterAttributeInfo> GetAttribute(CharacterAttribute attribute, BitmapImage icon, TextBinding unit, int digits)
        {
            return KeyValuePair.Create(attribute.ToString(), CharacterAttributeInfo.Create(attribute.ToString(), icon, unit, digits));
        }

        static CharacterAttributeExtension()
        {
            TextBinding LevelUnit = MarkedTextManage.Binding(nameof(MarkedText.UnitLevel));
            TextBinding ModuloUnit = MarkedTextManage.Binding(nameof(MarkedText.UnitModulo));
            AttributeMap = FrozenDictionary.Create([
                GetAttribute(CharacterAttribute.Attack,                 AttributeImage.Attack,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.AttackBase,             AttributeImage.Attack,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.Health,                 AttributeImage.Health,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.HealthBase,             AttributeImage.Health,      TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.Defense,                AttributeImage.Defense,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.DefenseBase,            AttributeImage.Defense,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.Speed,                  AttributeImage.Speed,       TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.SpeedBase,              AttributeImage.Speed,       TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.CriticalHitRate,        AttributeImage.Critical,    ModuloUnit,             1),
                GetAttribute(CharacterAttribute.CriticalHitDamage,      AttributeImage.Offense,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.ElementIncrease,        AttributeImage.Unknown,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.DefenseDecrease,        AttributeImage.Defense,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.MagicalDecrease,        AttributeImage.Unknown,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.BreakEffect,            AttributeImage.Break,       ModuloUnit,             1),
                GetAttribute(CharacterAttribute.BreakEfficiency,        AttributeImage.Break,       ModuloUnit,             1),
                GetAttribute(CharacterAttribute.BreakIncrease,          AttributeImage.Break,       ModuloUnit,             1),
                GetAttribute(CharacterAttribute.ElationIncrease,        AttributeImage.Punchline,   ModuloUnit,             1),
                GetAttribute(CharacterAttribute.ToughDecline,           AttributeImage.Maximum,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.EffectHitRate,          AttributeImage.HitRate,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.EffectMagical,          AttributeImage.Magical,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.HealingBoost,           AttributeImage.Healing,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.ElationBonus,           AttributeImage.Punchline,   TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.ManaReplenish,          AttributeImage.Replenish,   ModuloUnit,             1),
                GetAttribute(CharacterAttribute.MaximumEnergy,          AttributeImage.Maximum,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.WonsterLevel,           AttributeImage.Health,      LevelUnit,              0),
                GetAttribute(CharacterAttribute.MonsterLevel,           AttributeImage.Health,      LevelUnit,              0),
                GetAttribute(CharacterAttribute.MonsterCount,           AttributeImage.Unknown,     TextBinding.Default,    0),
                GetAttribute(CharacterAttribute.MagicalIncrease,        AttributeImage.Unknown,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.DamageDecrease,         AttributeImage.Defense,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.DamageIncrease,         AttributeImage.Unknown,     ModuloUnit,             1),
                GetAttribute(CharacterAttribute.Toughness,              AttributeImage.Maximum,     TextBinding.Default,    0),
                ]);
        }
    }
}