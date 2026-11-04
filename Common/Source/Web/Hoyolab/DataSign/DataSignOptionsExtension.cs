using Common.Source.Service.Encode.Hashing;
using System.Security.Cryptography;

namespace Common.Source.Web.Hoyolab.DataSign
{
    public static class DataSignOptionsExtension
    {
        public static string GetDataSign(this DataSignOptions options)
        {
            long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string content = $"salt={options.Salt}&t={unixTime}&r={options.RandomString}";
            if (options.Algorithm >= DataSignAlgorithm.Gen2)
            {
                content = $"{content}&b={options.Body}&q={options.Query}";
            }
            string check = HashMethod.ToHexStringLower(HashAlgorithmName.MD5, content);
            return $"{unixTime},{options.RandomString},{check}";
        }
    }
}