using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Model.Collection.Token;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab
{
    public static class HoyolabTokenManage
    {
        private static readonly TokenManager<HoyolabToken> Manager;

        public static HoyolabToken[] Tokens
        {
            get => Manager.Tokens;
            set => Manager.Tokens = value;
        }

        public static HoyolabToken[] Load()
        {
            return Manager.Load();
        }

        public static void Save(HoyolabToken[] tokens)
        {
            Manager.Save(tokens);
        }

        public static void Update(HoyolabToken token)
        {
            Manager.Update(token);
        }

        public static bool TryGetTokenOrFirst(string? aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return string.IsNullOrEmpty(aid) ? Manager.Tokens.TryGetFirst(out hoyolabToken) : TryGetToken(aid, out hoyolabToken);
        }

        public static bool TryGetToken(string aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return Manager.Tokens.TryGetFirst(current => current.Aid == aid, out hoyolabToken);
        }

        public static string GetGuid()
        {
            return Manager.Tokens.FirstOrDefault()?.Guid ?? Guid.NewGuid().ToString();
        }

        public static string GetFilePath()
        {
            return Path.Combine(LocalSetting.LocalPath, "HoyolabToken.json");
        }

        static HoyolabTokenManage()
        {
            Manager = new TokenManager<HoyolabToken>(GetFilePath(), HoyolabToken.Comparer);
        }
    }
}