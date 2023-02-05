using Guppy.Attributes;
using Guppy.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Resources.Loaders
{
    [Service<IPackLoader>(ServiceLifetime.Singleton, true)]
    public interface IPackLoader
    {
        void Load(IPackProvider packs);
    }
}
