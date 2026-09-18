namespace Common.Source.Web.EHentai
{
    public class EHentaiToken
    {
        public static IEqualityComparer<EHentaiToken> Comparer { get; }

        public string IpbMemberId { get; set; } = string.Empty;

        public string IpbPassHash { get; set; } = string.Empty;

        public string Igneous { get; set; } = string.Empty;

        public EHentaiToken() { }

        public EHentaiToken(string ipbMemberId)
        {
            IpbMemberId = ipbMemberId;
        }

        static EHentaiToken()
        {
            Comparer = EqualityComparer<EHentaiToken>.Create((sender, other) => sender?.IpbMemberId == other?.IpbMemberId, sender => sender.IpbMemberId.GetHashCode());
        }
    }
}