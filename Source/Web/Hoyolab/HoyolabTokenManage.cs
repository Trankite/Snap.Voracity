using StarRailDamage.Source.Core.Abstraction;
using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Encode.Encrypt;
using StarRailDamage.Source.Service.Encode.Hashing;
using StarRailDamage.Source.Service.FileOpen;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarRailDamage.Source.Web.Hoyolab
{
    public static class HoyolabTokenManage
    {
        private static HoyolabToken[]? _HoyolabTokens;

        private static readonly JsonSerializerOptions JsonOptions;

        private const string Salt = "B9176A0A08605E7EE16428AB13199AC2";

        public static HoyolabToken[] HoyolabTokens
        {
            get => _HoyolabTokens ?? Load().Captured(_HoyolabTokens);
            private set => _HoyolabTokens = value;
        }

        public static bool TryGetTokenOrFirst(string? aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return string.IsNullOrEmpty(aid) ? HoyolabTokens.TryGetFirst(out hoyolabToken) : TryGetToken(aid, out hoyolabToken);
        }

        public static bool TryGetToken(string aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return HoyolabTokens.TryGetFirst(Token => Token.Aid == aid, out hoyolabToken);
        }

        [MemberNotNull(nameof(_HoyolabTokens))]
        public static bool Load()
        {
            using FileOpenRead FileRead = new(GetFilePath());
            if (FileRead.Success)
            {
                return true.Configure(_HoyolabTokens = JsonSerializer.Deserialize<HoyolabToken[]>(FileRead.Stream, JsonOptions).NotNull());
            }
            return false.Configure(_HoyolabTokens = []);
        }

        public static bool Save(params HoyolabToken[] hoyolabTokens)
        {
            using FileOpenWrite FileWrite = FileOpenWrite.Create(GetFilePath());
            if (FileWrite.Success)
            {
                JsonSerializer.SerializeAsync(FileWrite.Stream, hoyolabTokens, JsonOptions).RunSynchronously();
                return true.Configure(_HoyolabTokens = hoyolabTokens);
            }
            return false;
        }

        public static string GetFilePath()
        {
            return Path.Combine(LocalSetting.LocalPath, "HoyolabToken.json");
        }

        private static AESAlgorithm GetAlgorithm()
        {
            return new AESAlgorithm(HashMethod.HashData(HashAlgorithmName.SHA256, AppSetting.UserSid + Salt));
        }

        static HoyolabTokenManage()
        {
            JsonOptions = JsonSerializerExtension.JsonOptions.Copy();
            JsonOptions.Converters.Add(new JsonHoyolabTokenConverter());
        }

        private class JsonHoyolabTokenConverter : JsonConverter<Dictionary<HoyolabTokenType, string>>, IEmployedEncoding
        {
            public Encoding Encoding => Encoding.UTF8;

            public override Dictionary<HoyolabTokenType, string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using AESAlgorithm Algorithm = GetAlgorithm();
                Dictionary<HoyolabTokenType, string> HoyolabTokens = [];
                while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (EnumExtension.TryParse(reader.GetString(), out HoyolabTokenType TokenType) && reader.Read())
                    {
                        string? Token = reader.GetString();
                        if (string.IsNullOrEmpty(Token)) continue;
                        try
                        {
                            HoyolabTokens[TokenType] = Encoding.GetString(Algorithm.DecryptFromBase64String(Token));
                        }
                        catch
                        {
                            HoyolabTokens[TokenType] = string.Empty;
                        }
                    }
                }
                return HoyolabTokens;
            }

            public override void Write(Utf8JsonWriter writer, Dictionary<HoyolabTokenType, string> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                using AESAlgorithm Algorithm = GetAlgorithm();
                foreach (KeyValuePair<HoyolabTokenType, string> Token in value)
                {
                    writer.WriteString(Token.Key.ToString(), Algorithm.EncryptToBase64String(Encoding.GetBytes(Token.Value)));
                }
                writer.WriteEndObject();
            }
        }
    }
}