using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Serialization.Common.Factories
{
    [Service<IDefaultInstanceFactory>(ServiceLifetime.Singleton, ServiceRegistrationFlags.RequireAutoLoadAttribute)]
    public interface IDefaultInstanceFactory
    {
        bool CanConstructType(Type type);

        object BuildInstance(Type type);
    }
}
