using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Sign
{
    public class SignResponse : ResponseWrapper<SignResponseWrapper>
    {
        public override bool Success => base.Success || Code == -5003;
    }
}