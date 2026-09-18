namespace Common.Source.Model.Collection.Token.Abstract
{
    public interface ITokenManager<T>
    {
        T[] Tokens { get; set; }

        ValueTask<T[]?> Load(CancellationToken cancellationToken = default);

        ValueTask Save(T[] tokens, CancellationToken cancellationToken = default);

        ValueTask Update(T token, CancellationToken cancellationToken = default);
    }
}