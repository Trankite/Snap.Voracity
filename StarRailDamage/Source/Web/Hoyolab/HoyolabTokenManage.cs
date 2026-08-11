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
            get
            {
                if (_HoyolabTokens.IsNull())
                {
                    _HoyolabTokens = Load().AsTask().Result.NotNull();
                }
                return _HoyolabTokens;
            }
            private set => _HoyolabTokens = value;
        }

        public static async ValueTask<HoyolabToken[]?> Load(CancellationToken cancellationToken = default)
        {
            using FileOpenRead FileRead = new(GetFilePath());
            if (!FileRead.Success) return default;
            return await JsonSerializerExtension.DeserializeAsync<HoyolabToken[]>(FileRead.Stream, default, cancellationToken);
        }

        public static async ValueTask Save(HoyolabToken[] hoyolabTokens, CancellationToken cancellationToken = default)
        {
            using FileOpenWrite FileWrite = FileOpenWrite.Create(GetFilePath());
            FileWrite.ThrowIfFailed();
            await JsonSerializerExtension.SerializeAsync(FileWrite.Stream, hoyolabTokens, default, cancellationToken);
            _HoyolabTokens = hoyolabTokens;
        }

        public static async ValueTask Update(HoyolabToken hoyolabToken)
        {
            if (HoyolabTokens.TryGetIndexOf(Current => Current.Aid == hoyolabToken.Aid, out int Index))
            {
                HoyolabTokens[Index] = hoyolabToken;
            }
            else
            {
                HoyolabTokens = [.. HoyolabTokens.Append(hoyolabToken).OrderBy(Current => Current.Aid)];
            }
            await Save(HoyolabTokens);
        }

        public static bool TryGetTokenOrFirst(string? aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return string.IsNullOrEmpty(aid) ? HoyolabTokens.TryGetFirst(out hoyolabToken) : TryGetToken(aid, out hoyolabToken);
        }

        public static bool TryGetToken(string aid, [NotNullWhen(true)] out HoyolabToken? hoyolabToken)
        {
            return HoyolabTokens.TryGetFirst(Token => Token.Aid == aid, out hoyolabToken);
        }

        public static string GetGuid()
        {
            return HoyolabTokens.FirstOrDefault()?.Guid ?? Guid.NewGuid().ToString();
        }

        public static string GetFilePath()
        {
            return Path.Combine(LocalSetting.LocalPath, "HoyolabToken.json");
        }
    }
}