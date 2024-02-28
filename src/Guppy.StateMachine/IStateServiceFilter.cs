using Guppy.StateMachine.Services;

namespace Guppy.StateMachine
{
    public interface IStateServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(IStateService states);
    }
}
