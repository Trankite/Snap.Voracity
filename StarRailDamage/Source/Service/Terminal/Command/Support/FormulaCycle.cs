using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct.Formula.Magical;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class FormulaCycle : CyclicTerminalCommand
    {
        public override string Name => "formula";

        protected override string HelpOverride => LocalString.ServiceTerminalSupportFormulaCycleHelp;

        protected override ITerminalResponse InvokeOverride(string? line)
        {
            MagicalFormulaParser FormulaParser = new();
            MagicalFormula? Formula = FormulaParser.Parse(line);
            if (Formula.IsNotNull())
            {
                double Number = MagicalFormulaSolver.GetValue(Formula);
                return new TerminalResponse(true, $"{Formula} = {Number}");
            }
            return new TerminalResponse(false);
        }
    }
}