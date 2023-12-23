using Autofac;

namespace Guppy.Providers
{
    public interface IGuppyProvider : IEnumerable<IGuppy>, IDisposable
    {
        ILifetimeScope Scope { get; }

        event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyCreated;
        event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyDestroyed;

        void Initialize();

        IGuppy Create(Type guppyType);

        T Create<T>()
            where T : class, IGuppy;
    }
}
