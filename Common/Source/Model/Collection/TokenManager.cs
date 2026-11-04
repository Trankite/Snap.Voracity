using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;

namespace Common.Source.Model.Collection
{
    public class TokenManager<T>
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
            using FileOpenRead fileRead = FileOpenRead.Create(FilePath);
            fileRead.ThrowIfFailed();
            return JsonSerializerExtension.Deserialize<T[]>(fileRead.Stream) ?? [];
        }

        public void Save(T[] tokens)
        {
            using FileOpenWrite fileWrite = FileOpenWrite.Create(FilePath);
            fileWrite.ThrowIfFailed();
            JsonSerializerExtension.Serialize(fileWrite.Stream, _Tokens = tokens);
        }

        public void Update(T token)
        {
            if (Tokens.TryGetIndexOf(token, out int index, Comparer))
            {
                Tokens[index] = token;
            }
            else
            {
                Tokens = [.. Tokens, token];
            }
            Save(Tokens);
        }
    }
}