using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Sign
{
    public class SignResponse : ResponseWrapper<SignResponseWrapper>
    {
        public override bool IsSuccess()
        {
            return base.IsSuccess() || Code == -5003;
        }
    }
}