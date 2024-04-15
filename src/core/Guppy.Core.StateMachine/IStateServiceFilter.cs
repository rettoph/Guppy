using Guppy.Core.StateMachine.Services;

namespace Guppy.Core.StateMachine
{
    public interface IStateServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(IStateService states);
    }
}
