using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Formula.Magical;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class FormulaCycle : CyclicTerminalCommand
    {
        public override string Name => "formula";

        public override string FullName => LocalString.ServiceTerminalSupportFormulaCycleFullName;

        protected override string HelpOverride => LocalString.ServiceTerminalSupportFormulaCycleHelp;

        protected override ITerminalResponse InvokeOverride(string? line)
        {
            MagicalFormulaParser FormulaParser = new();
            MagicalFormula? Formula = FormulaParser.Parse(line);
            if (Formula.IsNotNull())
            {
                MagicalFormulaSolver FormulaSolver = new();
                if (FormulaSolver.Verify(Formula, out string? Message))
                {
                    return new TerminalResponse(true, $"{Formula} = {FormulaSolver.GetValue(Formula)}");
                }
                else
                {
                    return new TerminalResponse(false, Message);
                }
            }
            return new TerminalResponse(false);
        }
    }
}