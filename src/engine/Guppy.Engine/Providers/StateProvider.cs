using Guppy.Engine.Attributes;
using Guppy.Engine.Enums;
using Guppy.StateMachine;
using Guppy.StateMachine.Providers;

namespace Guppy.Engine.Providers
{
    [Service<IStateProvider>(ServiceLifetime.Scoped, true)]
    public abstract class StateProvider : IStateProvider
    {
        public abstract IEnumerable<IState> GetStates();
    }
}
