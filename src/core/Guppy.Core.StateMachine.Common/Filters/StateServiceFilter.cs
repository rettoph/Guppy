using Autofac;
using Guppy.Core.Common;
using Guppy.Core.StateMachine.Common.Services;

namespace Guppy.Core.StateMachine.Common.Filters
{
    public abstract class StateServiceFilter : IServiceFilter
    {
        public abstract bool AppliesTo(Type type);

        public bool Invoke(ILifetimeScope scope)
        {
            return this.Invoke(scope.Resolve<IStateService>());
        }

        public abstract bool Invoke(IStateService state);
    }

    public class StateServiceFilter<TState> : StateServiceFilter
    {
        public readonly Type ServiceType;

        public readonly IStateKey<TState> Key;
        public readonly TState Value;

        public StateServiceFilter(Type serviceType, IStateKey<TState> key, TState value)
        {
            this.ServiceType = serviceType;
            this.Key = key;
            this.Value = value;
        }

        public override bool AppliesTo(Type type)
        {
            if (this.ServiceType.IsGenericTypeDefinition && type.ImplementsGenericTypeDefinition(this.ServiceType))
            {
                return true;
            }

            bool result = this.ServiceType.IsAssignableFrom(type);

            return result;
        }

        public override bool Invoke(IStateService states)
        {
            bool result = states.Matches(this.Key, this.Value);
            return result;
        }
    }
}
