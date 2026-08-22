using Common.Source.Extension;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin.Status
{
    public class QRLoginStatusResponse : ResponseWrapper<QRLoginStatusResponseWrapper>
    {
        public QRLoginStatus GetStatus()
        {
            return Content.IsNotNull() && EnumExtension.TryParse(Content.Status, out QRLoginStatus Status) ? Status : default;
        }
    }
}