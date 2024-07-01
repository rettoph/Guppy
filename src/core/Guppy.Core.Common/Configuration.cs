using Autofac;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common
{
    internal class Configuration<T> : IConfiguration<T>
        where T : new()
    {
        public T Value { get; }

        public Configuration(IConfigurationService configurations)
        {
            this.Value = new T();
            configurations.Configure(this.Value);
        }
    }

    internal abstract class ConfigurationBuilder
    {
        public abstract bool CanBuild(Type type);
        public abstract void Build(ILifetimeScope scope, object instance);
    }

    internal class ConfigurationBuilder<T> : ConfigurationBuilder
    {
        private Action<ILifetimeScope, T> _builder;

        public ConfigurationBuilder(Action<ILifetimeScope, T> builder)
        {
            _builder = builder;
        }

        public override bool CanBuild(Type type)
        {
            return type.IsAssignableTo<T>();
        }

        public override void Build(ILifetimeScope scope, object instance)
        {
            if (instance is not T casted)
            {
                return;
            }

            _builder(scope, casted);
        }
    }
}
