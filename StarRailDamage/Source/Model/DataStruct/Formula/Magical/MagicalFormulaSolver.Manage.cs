using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private static readonly MagicalFormulaSymbol[] SymbolTable;

        public static MagicalFormulaSymbol? SearchSymbol(ReadOnlySpan<char> target)
        {
            char BeginChar = target.FirstOrDefault();
            int Index = SymbolTable.BinarySearch(0, SymbolTable.Length, (Self, Index) => Self[Index].Name, target, (Content, Search) => Content.AsSpan().CompareTo(Search, StringComparison.Ordinal));
            Index = Index < 0 ? ~Index : Index + 1;
            while (--Index >= 0 && SymbolTable[Index].Name.StartsWith(BeginChar))
            {
                if (target.StartsWith(SymbolTable[Index].Name)) return SymbolTable[Index];
            }
            return null;
        }

        private static MagicalFormulaSymbol[] GetSymbolTable(params MagicalFormulaSymbol[] symbols)
        {
            return symbols.Configure(Self => Array.Sort(Self, (Current, Compare) => Current.Name.CompareTo(Compare.Name, StringComparison.Ordinal)));
        }

        static MagicalFormulaSolver()
        {
            SymbolTable = GetSymbolTable
            (
                new StartSymbol(),
                new EndedSymbol(),
                new SeparatorSymbol(),
                new SetSymbol(),
                new SetAddSymbol(),
                new SetSubtractSymbol(),
                new SetMultiplySymbol(),
                new SetDivideSymbol(),
                new OrSymbol(),
                new AndSymbol(),
                new MoreSymbol(),
                new MoreOrEqualSymbol(),
                new EqualSymbol(),
                new NotEqualSymbol(),
                new LessSymbol(),
                new LessOrEqualSymbol(),
                new AddSymbol(),
                new SubtractSymbol(),
                new MultiplySymbol(),
                new DivideSymbol(),
                new PowerSymbol(),
                new NotSymbol(),
                new HundredSymbol(),
                new ModuloSymbol(),
                new MaximumSymbol(),
                new MinimumSymbol(),
                new SwitchSymbol()
            );
        }
    }
}