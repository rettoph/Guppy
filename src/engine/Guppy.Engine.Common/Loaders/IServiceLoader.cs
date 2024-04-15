using Autofac;
using Guppy.Engine.Common.Attributes;

namespace Guppy.Engine.Common.Loaders
{
    [ServiceLoaderAutoLoad]
    public interface IServiceLoader
    {
        void ConfigureServices(ContainerBuilder services);
    }
}
