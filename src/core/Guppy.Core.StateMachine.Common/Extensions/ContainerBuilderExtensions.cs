using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common.Filters;

namespace Guppy.Core.StateMachine.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterStateFilter<TState>(this ContainerBuilder builder, Type serviceType, IStateKey<TState> key, TState value)
        {
            return builder.RegisterFilter(new StateServiceFilter<TState>(serviceType, key, value));
        }

        public static ContainerBuilder RegisterStateFilter<TState>(this ContainerBuilder builder, Type serviceType, TState value)
        {
            return builder.RegisterStateFilter(serviceType, StateKey<TState>.Default, value);
        }

        public static ContainerBuilder RegisterStateFilter<TService, TState>(this ContainerBuilder builder, IStateKey<TState> key, TState value)
        {
            return builder.RegisterStateFilter(typeof(TService), key, value);
        }

        public static ContainerBuilder RegisterStateFilter<TService, TState>(this ContainerBuilder builder, TState value)
        {
            return builder.RegisterStateFilter(typeof(TService), value);
        }
    }
}
