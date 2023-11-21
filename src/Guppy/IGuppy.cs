using Autofac;

namespace Guppy
{
    public interface IGuppy : IDisposable
    {
        IGuppyComponent[] Components { get; }

        event OnEventDelegate<IDisposable>? OnDispose;

        void Initialize(ILifetimeScope scope);
    }
}
