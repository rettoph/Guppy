using Autofac;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common.Extensions.Autofac;

namespace Guppy.Core.Commands.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreCommandServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreCommandServices)))
            {
                return builder;
            }


            builder.RegisterType<CommandService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            return builder.AddTag(nameof(RegisterCoreCommandServices));
        }
    }
}
