using Autofac;

namespace Guppy.Engine.Common.Providers
{
    public interface IGuppyProvider : IEnumerable<IGuppy>, IDisposable
    {
        ILifetimeScope Scope { get; }

        event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyCreated;
        event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyDestroyed;

        void Initialize();

        IGuppy Create(Type guppyType, Action<ContainerBuilder>? builder = null);

        T Create<T>(Action<ContainerBuilder>? builder = null)
            where T : class, IGuppy;
    }
}
