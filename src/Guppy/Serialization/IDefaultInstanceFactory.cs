using Guppy.Attributes;
using Guppy.Enums;

namespace Guppy.Serialization
{
    [Service<IDefaultInstanceFactory>(ServiceLifetime.Singleton, true)]
    public interface IDefaultInstanceFactory
    {
        bool CanConstructType(Type type);

        object BuildInstance(Type type);
    }
}
