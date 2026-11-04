using Common.Source.Factory.Serialize.Interface;

namespace Common.Source.Factory.Serialize.Abstract
{
    public abstract class ContentSerializerAsync<TDestination> : IContentSerializerAsync<TDestination>
    {
        public virtual TDestination Serialize()
        {
            return SerializeAsync().AsTask().GetAwaiter().GetResult();
        }

        public abstract ValueTask<TDestination> SerializeAsync(CancellationToken cancellationToken = default);
    }

    public abstract class ContentSerializerAsync<TDestination, TSource> : ContentSerializerAsync<TDestination>, IContentSerializerAsync<TDestination, TSource>
    {
        public virtual TSource Deserialize(TDestination source)
        {
            return DeserializeAsync(source).AsTask().GetAwaiter().GetResult();
        }

        public abstract ValueTask<TSource> DeserializeAsync(TDestination source, CancellationToken cancellationToken = default);
    }
}