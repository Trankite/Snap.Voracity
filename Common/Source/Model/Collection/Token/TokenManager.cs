using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Model.Collection.Token.Abstract;

namespace Common.Source.Model.Collection.Token
{
    public class TokenManager<T> : ITokenManager<T>
    {
        private T[]? _Tokens;

        private readonly string FilePath;

        private readonly IEqualityComparer<T>? Comparer;

        public T[] Tokens
        {
            get
            {
                if (_Tokens.IsNull())
                {
                    try
                    {
                        _Tokens = Load();
                    }
                    catch
                    {
                        _Tokens = [];
                    }
                }
                return _Tokens;
            }
            set => _Tokens = value;
        }

        public TokenManager(string filePath, IEqualityComparer<T>? comparer = default)
        {
            FilePath = filePath;
            Comparer = comparer;
        }

        public T[] Load()
        {
            using FileOpenRead FileRead = FileOpenRead.Create(FilePath);
            FileRead.ThrowIfFailed();
            return JsonSerializerExtension.Deserialize<T[]>(FileRead.Stream) ?? [];
        }

        public void Save(T[] tokens)
        {
            using FileOpenWrite FileWrite = FileOpenWrite.Create(FilePath);
            FileWrite.ThrowIfFailed();
            JsonSerializerExtension.Serialize(FileWrite.Stream, _Tokens = tokens);
        }

        public void Update(T token)
        {
            if (Tokens.TryGetIndexOf(token, out int Index, Comparer))
            {
                Tokens[Index] = token;
            }
            else
            {
                Tokens = [.. Tokens, token];
            }
            Save(Tokens);
        }
    }
}