using Guppy.Attributes;
using Guppy.Enums;

namespace Guppy.Loaders
{
    [Service<IGuppyLoader>(ServiceLifetime.Scoped, true)]
    public interface IGuppyLoader
    {
        void Load(IGuppy guppy);
    }
}
