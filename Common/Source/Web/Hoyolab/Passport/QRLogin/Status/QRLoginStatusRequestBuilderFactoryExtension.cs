using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin.Status
{
    public static class QRLoginStatusRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static QRLoginStatusRequestBuilderFactory SetGuid(this QRLoginStatusRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Guid = value);
        }

        [DebuggerStepThrough]
        public static QRLoginStatusRequestBuilderFactory SetTicket(this QRLoginStatusRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Ticket = value);
        }
    }
}