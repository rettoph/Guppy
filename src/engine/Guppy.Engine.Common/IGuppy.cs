using Autofac;
using Guppy.Engine.Common.Components;

namespace Guppy.Engine.Common
{
    public interface IGuppy
    {
        ulong Id { get; }
        string Name { get; }

        IGuppyComponent[] Components { get; }
        ILifetimeScope Scope { get; }

        void Initialize(ILifetimeScope scope);

        public string? ToString()
        {
            return $"{this.Name} - {this.Id}";
        }
    }
}
