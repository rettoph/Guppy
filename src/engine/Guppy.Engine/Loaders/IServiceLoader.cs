using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Autofac;

namespace Guppy.Engine.Loaders
{
    [ServiceLoaderAutoLoad]
    public interface IServiceLoader
    {
        object LifetimeScopeTag => LifetimeScopeTags.EngineScope;

        void ConfigureServices(ContainerBuilder services);
    }
}
