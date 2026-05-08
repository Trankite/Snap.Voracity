using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Encode.Encrypt;
using StarRailDamage.Source.Service.Encode.Hashing;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace StarRailDamage.Source.Web.Hoyolab
{
    public static class HoyolabTokenExtension
    {
        private const string Salt = "B9176A0A08605E7EE16428AB13199AC2";

        public static bool TryGetUserRole(this HoyolabToken hoyolabToken, GameType value, [NotNullWhen(true)] out HoyolabUserRole? userRole)
        {
            for (int i = 0; i < hoyolabToken.UserRoles.Count; i++)
            {
                userRole = hoyolabToken.UserRoles[i];
                if (GameTypeExtension.TryGetGameType(userRole.Game, out GameType GameType))
                {
                    if (GameType.HasFlag(value)) return true;
                }
            }
            return false.Configure(userRole = default);
        }

        public static string GetToken(this HoyolabToken hoyolabToken, HoyolabTokenType tokenType)
        {
            using AESAlgorithm Algorithm = GetAlgorithm();
            if (hoyolabToken.Tokens.TryGetValue(tokenType, out string? Ciphertext))
            {
                try
                {
                    return Encoding.UTF8.GetString(Algorithm.DecryptFromBase64String(Ciphertext));
                }
                catch
                {
                    hoyolabToken.Tokens.Remove(tokenType);
                }
            }
            return string.Empty;
        }

        public static void SetToken(this HoyolabToken hoyolabToken, HoyolabTokenType tokenType, string token)
        {
            using AESAlgorithm Algorithm = GetAlgorithm();
            hoyolabToken.Tokens[tokenType] = Algorithm.EncryptToBase64String(Encoding.UTF8.GetBytes(token));
        }

        private static AESAlgorithm GetAlgorithm()
        {
            return new AESAlgorithm(HashMethod.HashData(HashAlgorithmName.SHA256, AppSetting.GetUserSid() + Salt));
        }
    }
}