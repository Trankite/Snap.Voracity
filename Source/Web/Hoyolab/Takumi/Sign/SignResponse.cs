using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Web.Hoyolab.Takumi.Sign
{
    public class SignResponse : ResponseWrapper<SignResponseWrapper>
    {
        public override bool IsSuccess()
        {
            return base.IsSuccess() || Code == -5003;
        }
    }
}