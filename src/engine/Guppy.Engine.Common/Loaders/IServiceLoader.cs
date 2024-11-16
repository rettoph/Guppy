using Autofac;
using Guppy.Engine.Common.Attributes;

namespace Guppy.Engine.Common.Loaders
{
    [RegisterServiceLoader]
    public interface IServiceLoader
    {
        void ConfigureServices(ContainerBuilder builder);
    }
}
