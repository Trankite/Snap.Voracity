using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab
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
                    _HoyolabTokens = Load().AsTask().GetAwaiter().GetResult().NotNull();
                }
                return _HoyolabTokens;
            }
            private set => _HoyolabTokens = value;
        }

        public static ValueTask<HoyolabToken[]?> Load(CancellationToken cancellationToken = default)
        {
            using FileOpenRead FileRead = new(GetFilePath());
            if (!FileRead.Success) return default;
            return JsonSerializerExtension.DeserializeAsync<HoyolabToken[]>(FileRead.Stream, default, cancellationToken);
        }

        public static async ValueTask Save(HoyolabToken[] hoyolabTokens, CancellationToken cancellationToken = default)
        {
            using FileOpenWrite FileWrite = FileOpenWrite.Create(GetFilePath());
            FileWrite.ThrowIfFailed();
            await JsonSerializerExtension.SerializeAsync(FileWrite.Stream, _HoyolabTokens = hoyolabTokens, default, cancellationToken);
        }

        public static async ValueTask Update(HoyolabToken hoyolabToken)
        {
            if (HoyolabTokens.TryGetIndexOf(hoyolabToken, out int Index, HoyolabToken.Comparer))
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