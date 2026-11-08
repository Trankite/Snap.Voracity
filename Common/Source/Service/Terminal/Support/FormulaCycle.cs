using Common.Source.Extension;
using Common.Source.Factory.Formula.Magical;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;

namespace Common.Source.Service.Terminal.Support
{
    public class FormulaCycle : TerminalCommandCyclic
    {
        public override string Name => "formula";

        public override string FullName => LocalString.ServiceTerminalSupportFormulaCycleFullName;

        protected override string HelpOverride => LocalString.ServiceTerminalSupportFormulaCycleHelp;

        protected override ITerminalResponse InvokeOverride(string? line, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            string? message = string.Empty;
            MagicalFormulaParser parser = new();
            MagicalFormulaSolver solver = new();
            MagicalFormula? formula = parser.Parse(line);
            if (formula.IsNotNull() && solver.Verify(formula, out message))
            {
                message = $"{formula}\x20=\x20{solver.GetValue(formula)}";
            }
            return new TerminalResponse(true, message);
        }
    }
}