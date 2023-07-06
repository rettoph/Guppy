using Autofac;
using Guppy.Attributes;

namespace Guppy.Loaders
{
    [ServiceLoaderAutoLoad]
    public interface IServiceLoader
    {
        void ConfigureServices(ContainerBuilder services);
    }
}
