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

        public static EHentaiToken[] Load()
        {
            return Manager.Load();
        }

        public static void Save(EHentaiToken[] tokens)
        {
            Manager.Save(tokens);
        }

        public static void Update(EHentaiToken token)
        {
            Manager.Update(token);
        }

        public static bool TryGetTokenOrFirst(string? id, [NotNullWhen(true)] out EHentaiToken? eHentaiToken)
        {
            return string.IsNullOrEmpty(id) ? Manager.Tokens.TryGetFirst(out eHentaiToken) : TryGetToken(id, out eHentaiToken);
        }

        public static bool TryGetToken(string id, [NotNullWhen(true)] out EHentaiToken? eHentaiToken)
        {
            return Manager.Tokens.TryGetFirst(current => current.IpbMemberId == id, out eHentaiToken);
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