using Common.Source.Extension;
using Common.Source.Factory.Formula.Magical;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Service.Terminal.Support
{
    public class FormulaCycle : CyclicTerminalCommand
    {
        public override string Name => "formula";

        public override string FullName => LocalString.ServiceTerminalSupportFormulaCycleFullName;

        protected override string HelpOverride => LocalString.ServiceTerminalSupportFormulaCycleHelp;

        protected override ITerminalResponse InvokeOverride(string? line, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            string? Message = string.Empty;
            MagicalFormulaParser Parser = new();
            MagicalFormulaSolver Solver = new();
            MagicalFormula? Formula = Parser.Parse(line);
            if (Formula.IsNotNull() && Solver.Verify(Formula, out Message))
            {
                Message = $"{Formula}\x20=\x20{Solver.GetValue(Formula)}";
            }
            return new TerminalResponse(true, Message);
        }
    }
}