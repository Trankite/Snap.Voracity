using Common.Source.Extension;
using Common.Source.Factory.Serialize.Interface;

namespace Common.Source.Factory.Serialize.Abstract
{
    public abstract class ContentSerializerAsync<TDestination> : IContentSerializerAsync<TDestination>
    {
        public virtual TDestination Serialize()
        {
            return SerializeAsync().GetAwaitResult();
        }

        public abstract ValueTask<TDestination> SerializeAsync(CancellationToken cancellationToken = default);
    }

    public abstract class ContentSerializerAsync<TDestination, TSource> : ContentSerializerAsync<TDestination>, IContentSerializerAsync<TDestination, TSource>
    {
        public virtual TSource Deserialize(TDestination source)
        {
            return DeserializeAsync(source).GetAwaitResult();
        }

        public abstract ValueTask<TSource> DeserializeAsync(TDestination source, CancellationToken cancellationToken = default);
    }
}