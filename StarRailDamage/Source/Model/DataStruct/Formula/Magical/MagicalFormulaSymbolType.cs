namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    [Flags]
    public enum MagicalFormulaSymbolType
    {
        Start = 1 << 0,
        Ended = 1 << 1,
        Separator = 1 << 2,
        Prefix = 1 << 3,
        Dyadic = 1 << 4,
        Suffix = 1 << 5,
        Affixe = Prefix | Suffix,
        Method = 1 << 6
    }
}