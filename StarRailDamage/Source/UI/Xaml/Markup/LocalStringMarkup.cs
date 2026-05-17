using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Globalization;
using System.Windows.Markup;

namespace StarRailDamage.Source.UI.Xaml.Markup
{
    [MarkupExtensionReturnType(typeof(string))]
    public sealed class LocalStringMarkup : MarkupExtension
    {
        public string Name { get; set; } = string.Empty;

        public string? CultureName { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return LocalString.ResourceManager.GetString(string.Intern(Name), string.IsNullOrEmpty(CultureName) ? LocalString.Culture : CultureInfo.GetCultureInfo(CultureName)).NotNull();
        }
    }
}