using Common.Source.Core.Setting;
using Common.Source.Service.Encode.Encrypt;
using Common.Source.Service.Encode.Hashing;
using System.Security.Cryptography;
using System.Text;

namespace Common.Source.Web.EHentai
{
    public static class EHentaiTokenExtension
    {
        private const string Salt = "F6C98E723070F43057CB319D3BA2EE12";

        public static string GetToken(this EHentaiToken eHentaiToken)
        {
            using AESAlgorithm Algorithm = GetAlgorithm();
            try
            {
                return Encoding.UTF8.GetString(Algorithm.DecryptFromBase64String(eHentaiToken.IpbPassHash));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static EHentaiToken SetToken(this EHentaiToken eHentaiToken, string token)
        {
            using AESAlgorithm Algorithm = GetAlgorithm();
            eHentaiToken.IpbPassHash = Algorithm.EncryptToBase64String(Encoding.UTF8.GetBytes(token));
            return eHentaiToken;
        }

        private static AESAlgorithm GetAlgorithm()
        {
            return new AESAlgorithm(HashMethod.HashData(HashAlgorithmName.SHA256, LocalSetting.GetUserSid() + Salt));
        }
    }
}