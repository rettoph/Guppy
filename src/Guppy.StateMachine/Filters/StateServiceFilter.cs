using Autofac;
using Guppy.Common;
using Guppy.StateMachine.Services;

namespace Guppy.StateMachine.Filters
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

        public readonly IState<TState> RequiredState;

        public StateServiceFilter(Type serviceType, IState<TState> requiredState)
        {
            this.ServiceType = serviceType;
            this.RequiredState = requiredState;
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
            bool result = states.Matches(this.RequiredState);
            return result;
        }
    }

    public class StateServiceFilter<TService, TState> : StateServiceFilter<TState>
    {
        public StateServiceFilter(IState<TState> requiredState) : base(typeof(TService), requiredState)
        {
        }
    }
}
