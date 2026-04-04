using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Sign
{
    public class SignResponse : ResponseWrapper<SignResponseWrapper>
    {
        public override string ToString()
        {
            return Code == 1034 ? MarkedText.HoyolabForumSignWrong1034 : base.ToString();
        }
    }
}