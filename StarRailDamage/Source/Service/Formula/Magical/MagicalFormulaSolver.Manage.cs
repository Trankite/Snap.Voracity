using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private static readonly MagicalFormulaSymbol[] SymbolTable;

        private const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;

        public static MagicalFormulaSymbol? SearchSymbol(ReadOnlySpan<char> target)
        {
            ReadOnlySpan<char> StartString = target.SplitAt(1).Start;
            int Index = SymbolTable.BinarySearch(0, SymbolTable.Length, (Self, Index) => Self[Index].Name, target, (Content, Search) => Content.CompareTo(Search, Comparison));
            Index = Index < 0 ? ~Index : Index + 1;
            while (--Index >= 0 && SymbolTable[Index].Name.StartsWith(StartString, Comparison))
            {
                if (target.StartsWith(SymbolTable[Index].Name, Comparison)) return SymbolTable[Index];
            }
            return null;
        }

        private static MagicalFormulaSymbol[] GetSymbolTable(params MagicalFormulaSymbol[] symbols)
        {
            return symbols.Configure(Self => Array.Sort(Self, (Current, Compare) => Current.Name.CompareTo(Compare.Name, Comparison)));
        }

        static MagicalFormulaSolver()
        {
            SymbolTable = GetSymbolTable
            (
                new StartSymbol(),
                new EndedSymbol(),
                new SeparatorSymbol(),
                new AssignSymbol(),
                new AssignAddSymbol(),
                new AssignSubSymbol(),
                new AssignMulSymbol(),
                new AssignDivSymbol(),
                new OrSymbol(),
                new OrJumpSymbol(),
                new AndSymbol(),
                new AndJumpSymbol(),
                new MoreSymbol(),
                new MoreOrEqualSymbol(),
                new EqualSymbol(),
                new NotEqualSymbol(),
                new LessSymbol(),
                new LessOrEqualSymbol(),
                new AddSymbol(),
                new SubSymbol(),
                new MulSymbol(),
                new DivSymbol(),
                new PowerSymbol(),
                new NotSymbol(),
                new HundredSymbol(),
                new RandomSymbol(),
                new MinimumSymbol(),
                new MaximumSymbol(),
                new ModuloSymbol(),
                new RoundSymbol(),
                new JumpSymbol(),
                new ClampSymbol()
            );
        }
    }
}