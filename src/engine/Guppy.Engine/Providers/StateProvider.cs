using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.StateMachine;
using Guppy.Core.StateMachine.Providers;

namespace Guppy.Engine.Providers
{
    [Service<IStateProvider>(ServiceLifetime.Scoped, true)]
    public abstract class StateProvider : IStateProvider
    {
        public abstract IEnumerable<IState> GetStates();
    }
}
