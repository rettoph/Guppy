using Autofac;

namespace Guppy.Engine
{
    public interface IGuppy : IDisposable
    {
        ulong Id { get; }
        string Name { get; }

        IGuppyComponent[] Components { get; }
        ILifetimeScope Scope { get; }

        event OnEventDelegate<IDisposable>? OnDispose;

        void Initialize(ILifetimeScope scope);

        public string? ToString()
        {
            return $"{this.Name} - {this.Id}";
        }
    }
}
