using Guppy.Engine.Attributes;
using Guppy.Engine.Enums;

namespace Guppy.Engine.Serialization
{
    [Service<IDefaultInstanceFactory>(ServiceLifetime.Singleton, true)]
    public interface IDefaultInstanceFactory
    {
        bool CanConstructType(Type type);

        object BuildInstance(Type type);
    }
}
