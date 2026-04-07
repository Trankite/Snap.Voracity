using StarRailDamage.Source.Core.LocalText.Fixed;
using StarRailDamage.Source.Model.Text;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Model.Metadata.Character.Attribute
{
    public class CharacterAttributeInfo
    {
        public TextBinding Name { get; }

        public BitmapImage Icon { get; }

        public TextBinding Unit { get; }

        public int Digits { get; }

        private CharacterAttributeInfo(TextBinding name, BitmapImage icon, TextBinding unit, int digits)
        {
            Name = name;
            Icon = icon;
            Unit = unit;
            Digits = digits;
        }

        public static CharacterAttributeInfo Create(string attribute, BitmapImage icon, TextBinding unit, int digits)
        {
            return new CharacterAttributeInfo(GetNameBinding(attribute), icon, unit, digits);
        }

        private static TextBinding GetNameBinding(string attribute)
        {
            return FixedTextManage.Binding("Attribute" + attribute);
        }
    }
}