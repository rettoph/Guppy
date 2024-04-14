using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;

namespace Guppy.Loaders
{
    [ServiceLoaderAutoLoad]
    public interface IServiceLoader
    {
        object LifetimeScopeTag => LifetimeScopeTags.EngineScope;

        void ConfigureServices(ContainerBuilder services);
    }
}
