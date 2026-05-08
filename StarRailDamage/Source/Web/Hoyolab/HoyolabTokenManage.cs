using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.FileOpen;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace StarRailDamage.Source.Web.Hoyolab
{
    public static class HoyolabTokenManage
    {
        private static HoyolabToken[]? _HoyolabTokens;

        public static HoyolabToken[] HoyolabTokens
        {
            get => _HoyolabTokens ?? Load().Captured(_HoyolabTokens);
            private set => _HoyolabTokens = value;
        }

        [MemberNotNull(nameof(_HoyolabTokens))]
        public static bool Load()
        {
            using FileOpenRead FileRead = new(GetFilePath());
            if (FileRead.Success)
            {
                return true.Configure(_HoyolabTokens = JsonSerializerExtension.Deserialize<HoyolabToken[]>(FileRead.Stream).NotNull());
            }
            return false.Configure(_HoyolabTokens = []);
        }

        public static bool Save(params HoyolabToken[] hoyolabTokens)
        {
            using FileOpenWrite FileWrite = FileOpenWrite.Create(GetFilePath());
            if (FileWrite.Success)
            {
                JsonSerializerExtension.SerializeAsync(FileWrite.Stream, hoyolabTokens).RunSynchronously();
                return true.Configure(_HoyolabTokens = hoyolabTokens);
            }
            return false;
        }

        public static bool TryGetTokenOrFirst(string? aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return string.IsNullOrEmpty(aid) ? HoyolabTokens.TryGetFirst(out hoyolabToken) : TryGetToken(aid, out hoyolabToken);
        }

        public static bool TryGetToken(string aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return HoyolabTokens.TryGetFirst(Token => Token.Aid == aid, out hoyolabToken);
        }

        public static string GetFilePath()
        {
            return Path.Combine(LocalSetting.LocalPath, "HoyolabToken.json");
        }
    }
}