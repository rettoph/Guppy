using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Resources.Providers;

namespace Guppy.Resources.Loaders
{
    [Service<IPackLoader>(ServiceLifetime.Singleton, true)]
    public interface IPackLoader
    {
        void Load(IResourcePackProvider packs);
    }
}
