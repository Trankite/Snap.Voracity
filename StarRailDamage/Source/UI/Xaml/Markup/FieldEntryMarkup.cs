using StarRailDamage.Source.Model.Metadata.Character.Attribute;
using StarRailDamage.Source.UI.Model.View;
using System.Windows.Markup;

namespace StarRailDamage.Source.UI.Xaml.Markup
{
    [MarkupExtensionReturnType(typeof(FieldEntrySpanModel))]
    public class FieldEntryMarkup : MarkupExtension
    {
        public string Name { get; set; } = string.Empty;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            CharacterAttributeInfo AttributeInfo = CharacterAttributeExtension.GetModel(Name);
            return new FieldEntrySpanModel(AttributeInfo.Icon, AttributeInfo.Name, string.Empty, AttributeInfo.Unit, AttributeInfo.Digits);
        }
    }
}