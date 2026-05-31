using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private static bool AssignSymbolVerify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
        {
            if (string.IsNullOrEmpty(formula.Start?.Content?.Target))
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMisuseAssignSymbol.Format(formula.Symbol.Name));
            }
            if (formula.Ended.IsNull())
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
            }
            return true.Configure(message = default);
        }

        private static bool PrefixSymbolVerify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
        {
            if (formula.Start.IsNotNull())
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMisuseAffixeSymbol.Format(formula.Symbol.Name));
            }
            if (formula.Ended.IsNull())
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
            }
            return true.Configure(message = default);
        }

        private static bool SuffixSymbolVerify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
        {
            if (formula.Ended.IsNotNull())
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMisuseAffixeSymbol.Format(formula.Symbol.Name));
            }
            if (formula.Start.IsNull())
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
            }
            return true.Configure(message = default);
        }
    }
}