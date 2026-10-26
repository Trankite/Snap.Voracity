using Common.Source.Extension;
using Common.Source.Model.Collection;

namespace Common.Source.Factory.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private static readonly SortedArray<MagicalFormulaSymbol> SymbolTable;

        private const StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase;

        public static MagicalFormulaSymbol? SearchSymbol(ReadOnlySpan<char> target)
        {
            ReadOnlySpan<char> StartString = target.SplitAt(1).Front;
            static int Comparison(MagicalFormulaSymbol sender, ReadOnlySpan<char> other)
            {
                return MemoryExtensions.CompareTo(sender.Name, other, ComparisonType);
            }
            int Index = SymbolTable.Search(target, Comparison).GetInsert(1);
            while (--Index >= 0 && SymbolTable[Index].Name.StartsWith(StartString, ComparisonType))
            {
                if (target.StartsWith(SymbolTable[Index].Name, ComparisonType))
                {
                    return SymbolTable[Index];
                }
            }
            return default;
        }

        private class SymbolComparer : IComparer<MagicalFormulaSymbol>
        {
            private readonly StringComparer Comparer;

            public SymbolComparer()
            {
                Comparer = StringComparer.FromComparison(ComparisonType);
            }

            public int Compare(MagicalFormulaSymbol? sender, MagicalFormulaSymbol? other)
            {
                return Comparer.Compare(sender?.Name, other?.Name);
            }
        }

        static MagicalFormulaSolver()
        {
            SymbolTable = SortedArray.Create
            (
                new SymbolComparer(),
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