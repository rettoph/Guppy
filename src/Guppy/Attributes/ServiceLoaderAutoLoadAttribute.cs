using Autofac;
using Guppy.Extensions.Autofac;

namespace Guppy.Attributes
{
    internal sealed class ServiceLoaderAutoLoadAttribute : AutoLoadingAttribute
    {
        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            builder.RegisterServiceLoader(classType);
        }
    }
}
