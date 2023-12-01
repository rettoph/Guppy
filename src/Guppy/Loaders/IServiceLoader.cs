using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;

namespace Guppy.Loaders
{
    [ServiceLoaderAutoLoad]
    public interface IServiceLoader
    {
        object LifetimeScopeTag => LifetimeScopeTags.MainScope;

        void ConfigureServices(ContainerBuilder services);
    }
}
