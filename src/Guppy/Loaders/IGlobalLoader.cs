using Guppy.Attributes;
using Guppy.Enums;

namespace Guppy.Loaders
{
    [Service<IGlobalLoader>(ServiceLifetime.Singleton, true)]
    public interface IGlobalLoader
    {
        void Load(GuppyEngine engine);
    }
}
