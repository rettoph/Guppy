using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.StateMachine.Common.Providers
{
    [Service<IStateProvider>(ServiceLifetime.Scoped, ServiceRegistrationFlags.RequireAutoLoadAttribute)]
    public interface IStateProvider
    {
        IEnumerable<IState> GetStates();
    }
}
