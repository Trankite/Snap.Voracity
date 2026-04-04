namespace StarRailDamage.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeAnalyzedBody
    {
        public int Today { get; set; }

        public int Count { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public SignHomeAnalyzedBody() { }

        public SignHomeAnalyzedBody(int today, int count, string name, string icon)
        {
            Today = today;
            Count = count;
            Name = name;
            Icon = icon;
        }
    }
}