using Guppy.Core.StateMachine.Common.Enums;

namespace Guppy.Core.StateMachine.Common.Providers
{
    public interface IStateProvider
    {
        bool TryGet(IStateKey key, out object? state);
        TryMatchResultEnum TryMatch(IStateKey key, object? value);
    }
}