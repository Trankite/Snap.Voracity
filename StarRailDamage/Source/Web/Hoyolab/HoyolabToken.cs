namespace StarRailDamage.Source.Web.Hoyolab
{
    public class HoyolabToken
    {
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
    }
}