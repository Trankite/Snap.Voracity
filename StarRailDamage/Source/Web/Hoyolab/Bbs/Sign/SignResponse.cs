using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Sign
{
    public class SignResponse : ResponseWrapper<SignResponseWrapper>
    {
        public override string ToString()
        {
            return Code == 1034 ? LocalString.WebHoyolabForumSignFailed1034 : base.ToString();
        }
    }
}