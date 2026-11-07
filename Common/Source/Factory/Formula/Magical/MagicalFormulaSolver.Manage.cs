using Common.Source.Extension;
using Common.Source.Model.Collection;
using Common.Source.Model.Interface;

namespace Common.Source.Factory.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private const StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase;

        private static readonly SortedArray<MagicalFormulaSymbol, ReadOnlySpan<char>> SymbolTable;

        public static MagicalFormulaSymbol? SearchSymbol(ReadOnlySpan<char> target)
        {
            int index = SymbolTable.Search(target).GetInsert(1);
            ReadOnlySpan<char> startString = target.SplitAt(1).Head;
            while (--index >= 0 && SymbolTable[index].Name.StartsWith(startString, ComparisonType))
            {
                if (target.StartsWith(SymbolTable[index].Name, ComparisonType))
                {
                    return SymbolTable[index];
                }
            }
            return default;
        }

        private class SymbolComparer : IComparer<MagicalFormulaSymbol, ReadOnlySpan<char>>
        {
            public int Compare(MagicalFormulaSymbol? sender, MagicalFormulaSymbol? other)
            {
                return string.Compare(sender?.Name, other?.Name, ComparisonType);
            }

            public int Compare(MagicalFormulaSymbol? sender, ReadOnlySpan<char> other)
            {
                return MemoryExtensions.CompareTo(sender?.Name, other, ComparisonType);
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