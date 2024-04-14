using Autofac;
using Guppy.Engine.Extensions.Autofac;

namespace Guppy.Engine.Attributes
{
    internal sealed class ServiceLoaderAutoLoadAttribute : AutoLoadingAttribute
    {
        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            builder.RegisterServiceLoader(classType);
        }
    }
}
