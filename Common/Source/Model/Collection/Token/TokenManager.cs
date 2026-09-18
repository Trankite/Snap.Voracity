using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Model.Collection.Token.Abstract;

namespace Common.Source.Model.Collection.Token
{
    public class TokenManager<T> : ITokenManager<T>
    {
        private readonly string FilePath;

        private readonly IEqualityComparer<T>? Comparer;

        public T[] Tokens
        {
            get => field ??= Load().AsTask().GetAwaiter().GetResult().NotNull(); set;
        }

        public TokenManager(string filePath, IEqualityComparer<T>? comparer = default)
        {
            FilePath = filePath;
            Comparer = comparer;
        }

        public async ValueTask<T[]?> Load(CancellationToken cancellationToken = default)
        {
            using FileOpenRead FileRead = new(FilePath);
            if (!FileRead.Success) return default;
            return await JsonSerializerExtension.DeserializeAsync<T[]>(FileRead.Stream, default, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask Save(T[] tokens, CancellationToken cancellationToken = default)
        {
            using FileOpenWrite FileWrite = FileOpenWrite.Create(FilePath);
            FileWrite.ThrowIfFailed();
            await JsonSerializerExtension.SerializeAsync(FileWrite.Stream, Tokens = tokens, default, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask Update(T token, CancellationToken cancellationToken = default)
        {
            if (Tokens.TryGetIndexOf(token, out int Index, Comparer))
            {
                Tokens[Index] = token;
            }
            else
            {
                Tokens = [.. Tokens, token];
            }
            await Save(Tokens, cancellationToken).ConfigureAwait(false);
        }
    }
}