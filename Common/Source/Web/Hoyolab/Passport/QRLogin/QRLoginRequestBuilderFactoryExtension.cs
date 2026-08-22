using Common.Source.Extension;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin
{
    public static class QRLoginRequestBuilderFactoryExtension
    {
        public static QRLoginRequestBuilderFactory SetGuid(this QRLoginRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Guid = value);
        }
    }
}