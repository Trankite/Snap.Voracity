using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Service.Encode.Encrypt;
using Common.Source.Service.Encode.Hashing;
using Common.Source.Web.Hoyolab.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Common.Source.Web.Hoyolab
{
    public static class HoyolabTokenExtension
    {
        private const string Salt = "3CD84D1619D4646CA3CC5BC34B4DDDB4";

        public static string GetToken(this HoyolabToken hoyolabToken, HoyolabTokenType tokenType)
        {
            using AESAlgorithm algorithm = GetAlgorithm();
            if (hoyolabToken.Tokens.TryGetValue(tokenType, out string? ciphertext))
            {
                try { return Encoding.UTF8.GetString(algorithm.DecryptFromBase64String(ciphertext)); } catch { }
            }
            return string.Empty;
        }

        public static void SetToken(this HoyolabToken hoyolabToken, HoyolabTokenType tokenType, string token)
        {
            using AESAlgorithm algorithm = GetAlgorithm();
            hoyolabToken.Tokens[tokenType] = algorithm.EncryptToBase64String(Encoding.UTF8.GetBytes(token));
        }

        private static AESAlgorithm GetAlgorithm()
        {
            return AESAlgorithm.Create(HashMethod.HashData(HashAlgorithmName.SHA256, LocalSetting.GetUserSid() + Salt));
        }

        public static bool TryGetUserRole(this HoyolabToken hoyolabToken, HoyolabApp value, [NotNullWhen(true)] out HoyolabUserRole? userRole)
        {
            for (int i = 0; i < hoyolabToken.UserRoles.Length; i++)
            {
                userRole = hoyolabToken.UserRoles[i];
                if (HoyolabAppExtension.TryGetValue(userRole.Game, out HoyolabApp hoyolabApp))
                {
                    if (value.HasFlag(hoyolabApp))
                    {
                        return true;
                    }
                }
            }
            return false.Configure(userRole = default);
        }
    }
}