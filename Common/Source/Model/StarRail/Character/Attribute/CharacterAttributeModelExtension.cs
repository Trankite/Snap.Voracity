using Common.Source.Extension;
using Common.Source.Model.StarRail.Character.Damage;
using Common.Source.Model.StarRail.Character.Element;

namespace Common.Source.Model.StarRail.Character.Attribute
{
    public static class CharacterAttributeModelExtension
    {
        private static readonly int[] SpecialDamageTable;

        public static void UpdateDamage(this ICharacterAttributeModel model, CharacterElement element, ICharacterDamageModel damage)
        {
            double Magical = 1 + 0.01 * (model.MagicalIncrease - model.MagicalDecrease).Clamp(-90, 100);
            double Defense = (20 + model.CharacterLevel) / (20 + model.CharacterLevel + (20 + model.MonsterLevel) * Math.Max(0, 1 - 0.01 * model.DefenseDecrease));
            double Multiplier = Magical * Defense * (1 + 0.01 * model.DamageIncrease) * (1 - model.DamageDecrease * 0.01);
            damage.Delay = damage.Normal * Multiplier;
            damage.Critical = damage.Normal * Multiplier * (1 + 0.01 * model.CriticalHitDamage) * (1 + 0.01 * model.ElementIncrease);
            double SpecialDamage = GetSpecialDamage(model.CharacterLevel.ToInt());
            damage.Elation = SpecialDamage * 2 * Multiplier * (1 + 0.01 * model.ElationIncrease) * (1 + 0.01 * model.CriticalHitDamage) * GetElationBonusFactor(model.ElationBonus.ToInt());
            double BreakMultiplier = Multiplier * (1 + 0.01 * model.BreakIncrease) * (1 + 0.01 * model.BreakEffect);
            damage.Break = SpecialDamage * BreakMultiplier * (model.Toughness / 20 - 0.5) * GetBreakDegree(element);
            damage.SuperBreak = SpecialDamage * BreakMultiplier * model.ToughDecline / 10 * (1 + 0.01 * model.BreakEfficiency);
        }

        public static double GetElationBonusFactor(int bonus) => 1 + bonus * 5 / (bonus + 240);

        public static double GetSpecialDamage(int level) => SpecialDamageTable.GetIndexValue(level - 1);

        public static double GetBreakDegree(CharacterElement element)
        {
            return element switch { CharacterElement.Quantum or CharacterElement.Imaginary => 0.5, CharacterElement.Ice or CharacterElement.Lightning => 1, CharacterElement.Wind => 1.5, CharacterElement.Fire or CharacterElement.Physical => 2, _ => 0 };
        }

        static CharacterAttributeModelExtension()
        {
            SpecialDamageTable =
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