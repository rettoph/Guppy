using Autofac;

namespace Guppy.Core.Common.Configurations
{
    public abstract class Configurator
    {
        public abstract bool CanBuild(Type type);
        public abstract void Configure(ILifetimeScope scope, object instance);
    }

    public class Configurator<T>(Action<ILifetimeScope, T> builder) : Configurator
    {
        private readonly Action<ILifetimeScope, T> _builder = builder;

        public override bool CanBuild(Type type)
        {
            return type.IsAssignableTo<T>();
        }

        public override void Configure(ILifetimeScope scope, object instance)
        {
            if (instance is not T casted)
            {
                return;
            }

            this._builder(scope, casted);
        }
    }
}