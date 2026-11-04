namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignAward
    {
        public int Today { get; set; }

        public int Count { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public SignAward() { }

        public SignAward(int today, int count, string name, string icon)
        {
            Today = today;
            Count = count;
            Name = name;
            Icon = icon;
        }
    }
}