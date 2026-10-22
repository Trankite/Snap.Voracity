namespace Common.Source.Model.Collection.Token.Abstract
{
    public interface ITokenManager<T>
    {
        T[] Tokens { get; set; }

        T[] Load();

        void Save(T[] tokens);

        void Update(T token);
    }
}