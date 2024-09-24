using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    [Service(ServiceLifetime.Singleton, ServiceRegistrationFlags.RequireAutoLoadAttribute | ServiceRegistrationFlags.AsImplementedInterfaces)]
    public interface IEngineComponent
    {
        [RequireSequenceGroup<InitializeComponentSequenceGroup>]
        void Initialize(IGuppyEngine engine);
    }
}
