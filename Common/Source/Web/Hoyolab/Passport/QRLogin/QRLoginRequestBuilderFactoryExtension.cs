using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin
{
    public static class QRLoginRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static QRLoginRequestBuilderFactory SetGuid(this QRLoginRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Guid = value);
        }
    }
}