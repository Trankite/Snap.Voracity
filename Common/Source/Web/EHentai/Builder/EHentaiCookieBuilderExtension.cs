using Common.Source.Extension;
using Common.Source.Web.Request.Builder;
using System.Diagnostics;

namespace Common.Source.Web.EHentai.Builder
{
    public static class EHentaiCookieBuilderExtension
    {
        [DebuggerStepThrough]
        public static EHentaiCookieBuilder SetIpbMemberId(this EHentaiCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("ipb_member_id", builder.EHentaiToken.IpbMemberId));
        }

        [DebuggerStepThrough]
        public static EHentaiCookieBuilder SetIpbPassHash(this EHentaiCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("ipb_pass_hash", builder.EHentaiToken.GetToken()));
        }

        [DebuggerStepThrough]
        public static EHentaiCookieBuilder SetIgneous(this EHentaiCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("igneous", builder.EHentaiToken.Igneous));
        }
    }
}