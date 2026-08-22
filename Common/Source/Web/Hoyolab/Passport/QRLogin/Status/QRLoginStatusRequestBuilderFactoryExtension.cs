using Common.Source.Extension;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin.Status
{
    public static class QRLoginStatusRequestBuilderFactoryExtension
    {
        public static QRLoginStatusRequestBuilderFactory SetGuid(this QRLoginStatusRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Guid = value);
        }

        public static QRLoginStatusRequestBuilderFactory SetTicket(this QRLoginStatusRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Ticket = value);
        }
    }
}