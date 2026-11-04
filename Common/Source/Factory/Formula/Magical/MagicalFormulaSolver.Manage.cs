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
            ReadOnlySpan<char> startString = target.SplitAt(1).Head;
            static int Comparison(MagicalFormulaSymbol sender, ReadOnlySpan<char> other)
            {
                return MemoryExtensions.CompareTo(sender.Name, other, ComparisonType);
            }
            int index = SymbolTable.Search(target, Comparison).GetInsert(1);
            while (--index >= 0 && SymbolTable[index].Name.StartsWith(startString, ComparisonType))
            {
                if (target.StartsWith(SymbolTable[index].Name, ComparisonType))
                {
                    return SymbolTable[index];
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