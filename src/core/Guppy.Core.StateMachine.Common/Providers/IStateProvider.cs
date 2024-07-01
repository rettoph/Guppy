using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.StateMachine.Common.Enums;

namespace Guppy.Core.StateMachine.Common.Providers
{
    [Service<IStateProvider>(ServiceLifetime.Scoped, ServiceRegistrationFlags.RequireAutoLoadAttribute)]
    public interface IStateProvider
    {
        bool TryGet(IStateKey key, out object? state);
        TryMatchResultEnum TryMatch(IStateKey key, object? value);
    }
}
