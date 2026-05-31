using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public class MagicalFormulaContent
    {
        public double Number { get; set; }

        public string? Target { get; set; }

        public MagicalFormulaContent() { }

        public MagicalFormulaContent(double number)
        {
            Number = number;
        }

        public MagicalFormulaContent(string? target)
        {
            Target = target;
        }

        public static MagicalFormulaContent? Create(ReadOnlySpan<char> content)
        {
            if (content.StartsWith('[') && content.EndsWith(']'))
            {
                return new MagicalFormulaContent(content[1..^1].ToString());
            }
            else
            {
                return double.TryParse(content, out double Number) ? new MagicalFormulaContent(Number) : null;
            }
        }

        public override string ToString()
        {
            return Target.IsNotNull() ? $"[{Target}]" : $"{Number}";
        }
    }
}