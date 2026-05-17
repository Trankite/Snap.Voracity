using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Model.Metadata.Character.Element
{
    public class CharacterElementModel
    {
        public string Name { get; }

        public string Break { get; }

        public BitmapImage Element { get; }

        public BitmapImage Offense { get; }

        public BitmapImage Defense { get; }

        private CharacterElementModel(string name, string @break, BitmapImage element, BitmapImage offense, BitmapImage defense)
        {
            Name = name;
            Break = @break;
            Element = element;
            Offense = offense;
            Defense = defense;
        }

        public static CharacterElementModel Create(string element)
        {
            return new CharacterElementModel(GetNameBinding(element), GetBreakBinding(element), GetElementImage(element), GetOffenseImage(element), GetDefenseImage(element));
        }

        private static string GetNameBinding(string element)
        {
            return LocalString.ResourceManager.GetString("CharacterElement" + element, LocalString.Culture).NotNull();
        }

        private static string GetBreakBinding(string element)
        {
            return LocalString.ResourceManager.GetString("CharacterDelayedDamage" + element, LocalString.Culture).NotNull();
        }

        private static BitmapImage GetElementImage(string element)
        {
            return new BitmapImage(new Uri($"/Source/Resource/Icon/Element/{element}.png", UriKind.Relative));
        }

        private static BitmapImage GetOffenseImage(string element)
        {
            return new BitmapImage(new Uri($"/Source/Resource/Icon/Element/Offense/{element}.png", UriKind.Relative));
        }

        private static BitmapImage GetDefenseImage(string element)
        {
            return new BitmapImage(new Uri($"/Source/Resource/Icon/Element/Defense/{element}.png", UriKind.Relative));
        }
    }
}