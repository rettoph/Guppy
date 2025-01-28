using Autofac;

namespace Guppy.Core.Common.Configurations
{
    public abstract class Configurator
    {
        public abstract bool CanBuild(Type type);
        public abstract void Configure(IGuppyScope scope, object instance);
    }

    public class Configurator<T>(Action<IGuppyScope, T> builder) : Configurator
    {
        private readonly Action<IGuppyScope, T> _builder = builder;

        public override bool CanBuild(Type type)
        {
            return type.IsAssignableTo<T>();
        }

        public override void Configure(IGuppyScope scope, object instance)
        {
            if (instance is not T casted)
            {
                return;
            }

            this._builder(scope, casted);
        }
    }
}