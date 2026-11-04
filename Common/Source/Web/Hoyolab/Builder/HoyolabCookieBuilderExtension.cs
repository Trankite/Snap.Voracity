using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request.Builder;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Builder
{
    public static class HoyolabCookieBuilderExtension
    {
        [DebuggerStepThrough]
        public static HoyolabCookieBuilder SetMid(this HoyolabCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("mid", builder.HoyolabToken.Mid));
        }

        [DebuggerStepThrough]
        public static HoyolabCookieBuilder SetStoken(this HoyolabCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("stoken", builder.HoyolabToken.GetToken(HoyolabTokenType.SToken)));
        }

        [DebuggerStepThrough]
        public static HoyolabCookieBuilder SetLtuid(this HoyolabCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("ltuid", builder.HoyolabToken.Aid));
        }

        [DebuggerStepThrough]
        public static HoyolabCookieBuilder SetLtoken(this HoyolabCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("ltoken", builder.HoyolabToken.GetToken(HoyolabTokenType.LToken)));
        }

        [DebuggerStepThrough]
        public static HoyolabCookieBuilder SetAccountMid(this HoyolabCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("account_mid_v2", builder.HoyolabToken.Mid));
        }

        [DebuggerStepThrough]
        public static HoyolabCookieBuilder SetCookieToken(this HoyolabCookieBuilder builder)
        {
            return builder.Configure(builder.SetCookie("cookie_token_v2", builder.HoyolabToken.GetToken(HoyolabTokenType.Cookie)));
        }
    }
}