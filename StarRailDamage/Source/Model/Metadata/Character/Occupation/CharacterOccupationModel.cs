using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Model.Metadata.Character.Occupation
{
    public class CharacterOccupationModel
    {
        public string Name { get; }

        public BitmapImage Icon { get; }

        private CharacterOccupationModel(string name, BitmapImage icon)
        {
            Name = name;
            Icon = icon;
        }

        public static CharacterOccupationModel Create(string occupation)
        {
            return new CharacterOccupationModel(GetNameBinding(occupation), GetOccupationImage(occupation));
        }

        private static string GetNameBinding(string occupation)
        {
            return LocalString.ResourceManager.GetString("CharacterOccupation" + occupation, LocalString.Culture).NotNull();
        }

        private static BitmapImage GetOccupationImage(string occupation)
        {
            return new BitmapImage(new Uri($"/Source/Resource/Icon/Occupation/{occupation}.png", UriKind.Relative));
        }
    }
}