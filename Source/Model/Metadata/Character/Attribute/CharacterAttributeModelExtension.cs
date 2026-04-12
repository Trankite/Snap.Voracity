using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.Metadata.Character.Damage;
using StarRailDamage.Source.Model.Metadata.Character.Element;

namespace StarRailDamage.Source.Model.Metadata.Character.Attribute
{
    public static class CharacterAttributeModelExtension
    {
        private static readonly int[] NormalTable;

        public static CharacterDamageModel Damage(this CharacterAttributeModel model, CharacterElement element, CharacterDamageModel damage)
        {
            double WonsterDefense = GetDefense(model.WonsterLevel);
            double MonsterDefense = GetDefense(model.MonsterLevel) * GetOffense(-model.DefenseDecrease);
            double Defense = WonsterDefense / (WonsterDefense + MonsterDefense);
            double Magical = GetOffense((model.MagicalIncrease - model.MagicalDecrease).Middle(-90, 100));
            double Creased = GetOffense(model.DamageIncrease) * GetOffense(-model.DamageDecrease) * Magical * Defense;
            double NormalDamage = damage.Delay = damage.Normal * Creased;
            double Critical = GetOffense(model.CriticalHitDamage);
            damage.Critical = NormalDamage * Critical * GetOffense(model.ElementIncrease);
            double EqualDamage = GetOffense(model.SuperBreakEqual) - 1;
            double Elation = 1 + model.ToughDecline * 5 / (model.ToughDecline + 240);
            NormalDamage = GetNormalDamage(model.WonsterLevel) * Creased * GetOffense(model.BreakIncrease);
            damage.Elation = NormalDamage * 2 * EqualDamage * Elation * Critical;
            NormalDamage *= GetOffense(model.BreakEffect);
            double Toughness = model.ToughDecline * GetOffense(model.BreakEfficiency) / 10;
            damage.Break = NormalDamage * (model.Toughness / 20 - 0.5) * GetBreakEqual(element);
            damage.SuperBreak = NormalDamage * EqualDamage * Toughness;
            return damage;
        }

        public static double GetNormalDamage(double level) => NormalTable[GetLevelIndex(level)];

        public static double GetBreakEqual(CharacterElement element)
        {
            return element switch { CharacterElement.Quantum or CharacterElement.Imaginary => 0.5, CharacterElement.Ice or CharacterElement.Lightning => 1, CharacterElement.Wind => 1.5, CharacterElement.Fire or CharacterElement.Physical => 2, _ => double.NaN };
        }

        public static int GetLevelIndex(double level) => Convert.ToInt32(level).Middle(1, 80) - 1;

        public static double GetOffense(double value) => value / 100 + 1;

        public static double GetDefense(double level) => level + 20;

        static CharacterAttributeModelExtension()
        {
            NormalTable =
            [
                54,     58,     62,     68,     71,     74,     77,     80,     83,     86,
                91,     97,     103,    108,    113,    119,    124,    129,    135,    140,
                149,    159,    168,    177,    187,    196,    205,    214,    222,    231,
                246,    261,    275,    289,    303,    316,    328,    340,    352,    364,
                408,    452,    495,    537,    578,    619,    659,    698,    737,    775,
                871,    965,    1056,   1146,   1233,   1318,   1402,   1483,   1563,   1640,
                1752,   1862,   1969,   2074,   2177,   2277,   2376,   2472,   2567,   2660,
                2780,   2899,   3015,   3128,   3240,   3349,   3457,   3562,   3666,   3768
            ];
        }
    }
}