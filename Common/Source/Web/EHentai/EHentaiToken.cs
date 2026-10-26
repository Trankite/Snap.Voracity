namespace Common.Source.Web.EHentai
{
    public class EHentaiToken : IEquatable<EHentaiToken>
    {
        public string IpbMemberId { get; set; } = string.Empty;

        public string IpbPassHash { get; set; } = string.Empty;

        public string Igneous { get; set; } = string.Empty;

        public EHentaiToken() { }

        public EHentaiToken(string ipbMemberId)
        {
            IpbMemberId = ipbMemberId;
        }

        public bool Equals(EHentaiToken? other)
        {
            return IpbMemberId.Equals(other?.IpbMemberId);
        }

        public override bool Equals(object? other)
        {
            return Equals(other as EHentaiToken);
        }

        public override int GetHashCode()
        {
            return IpbMemberId.GetHashCode();
        }
    }
}