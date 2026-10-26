using Common.Source.Web.Hoyolab.Metadata;

namespace Common.Source.Web.Hoyolab
{
    public class HoyolabToken : IEquatable<HoyolabToken>
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

        public bool Equals(HoyolabToken? other)
        {
            return Aid.Equals(other?.Aid);
        }

        public override bool Equals(object? other)
        {
            return Equals(other as HoyolabToken);
        }

        public override int GetHashCode()
        {
            return Aid.GetHashCode();
        }
    }
}