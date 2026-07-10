using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Web.Hoyolab.Passport.QRLogin
{
    public static class QRLoginRequestBuilderFactoryExtension
    {
        public static QRLoginRequestBuilderFactory SetGuid(this QRLoginRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Guid = value);
        }
    }
}