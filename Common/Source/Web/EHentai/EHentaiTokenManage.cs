using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Model.Collection.Token;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.EHentai
{
    public static class EHentaiTokenManage
    {
        public static readonly TokenManager<EHentaiToken> Manager;

        public static EHentaiToken[] Tokens
        {
            get => Manager.Tokens;
            set => Manager.Tokens = value;
        }

        public static ValueTask<EHentaiToken[]?> Load(CancellationToken cancellationToken = default)
        {
            return Manager.Load(cancellationToken);
        }

        public static ValueTask Save(EHentaiToken[] tokens, CancellationToken cancellationToken = default)
        {
            return Manager.Save(tokens, cancellationToken);
        }

        public static ValueTask Update(EHentaiToken token, CancellationToken cancellationToken = default)
        {
            return Manager.Update(token, cancellationToken);
        }

        public static bool TryGetTokenOrFirst(string? id, [NotNullWhen(true)] out EHentaiToken? eHentaiToken)
        {
            return string.IsNullOrEmpty(id) ? Manager.Tokens.TryGetFirst(out eHentaiToken) : TryGetToken(id, out eHentaiToken);
        }

        public static bool TryGetToken(string id, [NotNullWhen(true)] out EHentaiToken? eHentaiToken)
        {
            return Manager.Tokens.TryGetFirst(Current => Current.IpbMemberId == id, out eHentaiToken);
        }

        public static string GetFilePath()
        {
            return Path.Combine(LocalSetting.LocalPath, "EHentaiToken.json");
        }

        static EHentaiTokenManage()
        {
            Manager = new TokenManager<EHentaiToken>(GetFilePath(), EHentaiToken.Comparer);
        }
    }
}