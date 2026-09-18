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

        public static ValueTask<HoyolabToken[]?> Load(CancellationToken cancellationToken = default)
        {
            return Manager.Load(cancellationToken);
        }

        public static ValueTask Save(HoyolabToken[] tokens, CancellationToken cancellationToken = default)
        {
            return Manager.Save(tokens, cancellationToken);
        }

        public static ValueTask Update(HoyolabToken token, CancellationToken cancellationToken = default)
        {
            return Manager.Update(token, cancellationToken);
        }

        public static bool TryGetTokenOrFirst(string? aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return string.IsNullOrEmpty(aid) ? Manager.Tokens.TryGetFirst(out hoyolabToken) : TryGetToken(aid, out hoyolabToken);
        }

        public static bool TryGetToken(string aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return Manager.Tokens.TryGetFirst(Current => Current.Aid == aid, out hoyolabToken);
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