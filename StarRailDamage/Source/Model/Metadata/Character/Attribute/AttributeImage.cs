using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Model.Metadata.Character.Attribute
{
    public static class AttributeImage
    {
        public static readonly BitmapImage Attack = GetImage("Attack");

        public static readonly BitmapImage Health = GetImage("Health");

        public static readonly BitmapImage Defense = GetImage("Defense");

        public static readonly BitmapImage Speed = GetImage("Speed");

        public static readonly BitmapImage Critical = GetImage("Critical");

        public static readonly BitmapImage Offense = GetImage("Offense");

        public static readonly BitmapImage Magical = GetImage("Magical");

        public static readonly BitmapImage HitRate = GetImage("HitRate");

        public static readonly BitmapImage Punchline = GetImage("Punchline");

        public static readonly BitmapImage Replenish = GetImage("Replenish");

        public static readonly BitmapImage Maximum = GetImage("Maximum");

        public static readonly BitmapImage Break = GetImage("Break");

        public static readonly BitmapImage Healing = GetImage("Healing");

        public static readonly BitmapImage Unknown = GetImage("Unknown");

        private static BitmapImage GetImage(string attribute)
        {
            return new BitmapImage(new Uri($"/Source/Resource/Icon/Attribute/{attribute}.png", UriKind.Relative));
        }
    }
}