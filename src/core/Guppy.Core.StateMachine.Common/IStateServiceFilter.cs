using Guppy.Core.StateMachine.Common.Services;

namespace Guppy.Core.StateMachine.Common
{
    public interface IStateServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(IStateService states);
    }
}
