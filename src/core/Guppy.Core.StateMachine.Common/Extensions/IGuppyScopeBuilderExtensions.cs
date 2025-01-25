using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.StateMachine.Common.Filters;

namespace Guppy.Core.StateMachine.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterStateFilter<TState>(this IGuppyScopeBuilder builder, Type serviceType, IStateKey<TState> key, TState value)
        {
            return builder.RegisterFilter(new StateServiceFilter<TState>(serviceType, key, value));
        }

        public static IGuppyScopeBuilder RegisterStateFilter<TState>(this IGuppyScopeBuilder builder, Type serviceType, TState value)
        {
            return builder.RegisterStateFilter(serviceType, StateKey<TState>.Default, value);
        }

        public static IGuppyScopeBuilder RegisterStateFilter<TService, TState>(this IGuppyScopeBuilder builder, IStateKey<TState> key, TState value)
        {
            return builder.RegisterStateFilter(typeof(TService), key, value);
        }

        public static IGuppyScopeBuilder RegisterStateFilter<TService, TState>(this IGuppyScopeBuilder builder, TState value)
        {
            return builder.RegisterStateFilter(typeof(TService), value);
        }
    }
}