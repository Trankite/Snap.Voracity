namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    [Flags]
    public enum MagicalFormulaSymbolType
    {
        Begin = 1 << 0,
        Ended = 1 << 1,
        Prefix = 1 << 2,
        Suffix = 1 << 3,
        Method = 1 << 4
    }
}