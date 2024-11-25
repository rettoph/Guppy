using Autofac;

namespace Guppy.Core.Commands.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCommand<T>(this ContainerBuilder builder)
            where T : ICommand
        {
            builder.RegisterType<T>().As<ICommand>().SingleInstance();

            return builder;
        }
    }
}
