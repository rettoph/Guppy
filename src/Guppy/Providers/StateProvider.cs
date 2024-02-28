using Guppy.Attributes;
using Guppy.Enums;
using Guppy.StateMachine;
using Guppy.StateMachine.Providers;

namespace Guppy.Providers
{
    [Service<IStateProvider>(ServiceLifetime.Scoped, true)]
    public abstract class StateProvider : IStateProvider
    {
        public abstract IEnumerable<IState> GetStates();
    }
}
