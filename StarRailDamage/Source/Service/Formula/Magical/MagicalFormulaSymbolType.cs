namespace StarRailDamage.Source.Service.Formula.Magical
{
    [Flags]
    public enum MagicalFormulaSymbolType
    {
        Default = 1 << 0,
        Start = 1 << 1,
        Ended = 1 << 2,
        Separator = 1 << 3,
        Prefix = 1 << 4,
        Dyadic = 1 << 5,
        Suffix = 1 << 6,
        Affixe = Prefix | Suffix,
        Method = 1 << 7
    }
}