using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    [Service(ServiceLifetime.Singleton, ServiceRegistrationFlags.RequireAutoLoadAttribute | ServiceRegistrationFlags.AsImplementedInterfaces)]
    public interface IEngineComponent
    {
        void Initialize();
    }
}
