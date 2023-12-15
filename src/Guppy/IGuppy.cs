using Autofac;

namespace Guppy
{
    public interface IGuppy : IDisposable
    {
        long Id { get; }
        string Name { get; }

        IGuppyComponent[] Components { get; }

        event OnEventDelegate<IDisposable>? OnDispose;

        void Initialize(ILifetimeScope scope);

        public string? ToString()
        {
            return $"{this.Name} - {this.Id}";
        }
    }
}
