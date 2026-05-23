namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public class MagicalFormulaContent
    {
        public double Number { get; set; }

        public string Target { get; set; } = string.Empty;

        public MagicalFormulaContent() { }

        public MagicalFormulaContent(double number)
        {
            Number = number;
        }

        public MagicalFormulaContent(string target)
        {
            Target = target;
        }

        public static MagicalFormulaContent Create(ReadOnlySpan<char> value)
        {
            return double.TryParse(value, out double Number) ? new MagicalFormulaContent(Number) : new MagicalFormulaContent(value.ToString());
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Target) ? Number.ToString() : Target;
        }
    }
}