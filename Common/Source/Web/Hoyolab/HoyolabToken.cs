namespace Common.Source.Web.Hoyolab
{
    public class HoyolabToken
    {
        public static IEqualityComparer<HoyolabToken> Comparer { get; }

        public string Aid { get; set; } = string.Empty;

        public string Mid { get; set; } = string.Empty;

        public string Device { get; set; } = string.Empty;

        public string Guid { get; set; } = string.Empty;

        public Dictionary<HoyolabTokenType, string> Tokens { get; set; } = [];

        public HoyolabUserRole[] UserRoles { get; set; } = [];

        public HoyolabToken() { }

        public HoyolabToken(string guid)
        {
            Guid = guid;
        }

        static HoyolabToken()
        {
            Comparer = EqualityComparer<HoyolabToken>.Create((sender, other) => ReferenceEquals(sender, other) || sender?.Aid == other?.Aid, sender => sender.Aid.GetHashCode());
        }
    }
}