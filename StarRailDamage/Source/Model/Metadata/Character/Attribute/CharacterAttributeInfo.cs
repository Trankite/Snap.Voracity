using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Model.Metadata.Character.Attribute
{
    public class CharacterAttributeInfo
    {
        public string Name { get; }

        public BitmapImage Icon { get; }

        public string Unit { get; }

        public int Digits { get; }

        private CharacterAttributeInfo(string name, BitmapImage icon, string unit, int digits)
        {
            Name = name;
            Icon = icon;
            Unit = unit;
            Digits = digits;
        }

        public static CharacterAttributeInfo Create(string attribute, BitmapImage icon, string unit, int digits)
        {
            return new CharacterAttributeInfo(GetNameBinding(attribute), icon, unit, digits);
        }

        private static string GetNameBinding(string attribute)
        {
            return LocalString.ResourceManager.GetString("CharacterAttribute" + attribute, LocalString.Culture).NotNull();
        }
    }
}