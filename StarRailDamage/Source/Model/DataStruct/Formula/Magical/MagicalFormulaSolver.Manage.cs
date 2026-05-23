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
            return symbols.Configure(Self => Array.Sort(Self, (Current, Pending) => Current.Name.CompareTo(Pending.Name, StringComparison.Ordinal)));
        }

        static MagicalFormulaSolver()
        {
            SymbolTable = GetSymbolTable
            (
                new BeginMethod(),
                new EndedMethod(),
                new SeparatorMethod(),
                new SetMethod(),
                new SetAddMethod(),
                new SetSubtractMethod(),
                new SetMultiplyMethod(),
                new SetDivideMethod(),
                new OrMethod(),
                new AndMethod(),
                new MoreMethod(),
                new MoreOrEqualMethod(),
                new EqualMethod(),
                new NotEqualMethod(),
                new LessMethod(),
                new LessOrEqualMethod(),
                new AddMethod(),
                new SubtractMethod(),
                new MultiplyMethod(),
                new DivideMethod(),
                new PowerMethod(),
                new ModuloMethod(),
                new MaximumMethod(),
                new MinimumMethod(),
                new SwitchMethod()
            );
        }
    }
}