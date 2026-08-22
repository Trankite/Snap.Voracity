using Common.Source.Resource.Localization;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Bbs.Sign
{
    public class SignResponse : ResponseWrapper<SignResponseWrapper>
    {
        public override string ToString()
        {
            return Code == 1034 ? LocalString.WebHoyolabForumSignFailed1034 : base.ToString();
        }
    }
}