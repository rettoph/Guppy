using Autofac;

namespace Guppy
{
    public interface IGuppy : IDisposable
    {
        Guid Id { get; }
        string Name { get; }

        IGuppyComponent[] Components { get; }

        event OnEventDelegate<IDisposable>? OnDispose;

        void Initialize(ILifetimeScope scope);
    }
}
