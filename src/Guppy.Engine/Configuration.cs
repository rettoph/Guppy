using Autofac;
using Guppy.Engine.Common;

namespace Guppy.Engine
{
    internal class Configuration<T> : IConfiguration<T>
        where T : new()
    {
        public T Value { get; }

        public Configuration(ILifetimeScope scope, IEnumerable<ConfigurationBuilder<T>> builders)
        {
            this.Value = new();

            foreach (var builder in builders)
            {
                builder.Build(scope, this.Value);
            }
        }
    }

    internal class ConfigurationBuilder<T>
        where T : new()
    {
        private Action<ILifetimeScope, T> _builder;

        public ConfigurationBuilder(Action<ILifetimeScope, T> builder)
        {
            _builder = builder;
        }

        public void Build(ILifetimeScope scope, T instance)
        {
            _builder(scope, instance);
        }
    }
}
