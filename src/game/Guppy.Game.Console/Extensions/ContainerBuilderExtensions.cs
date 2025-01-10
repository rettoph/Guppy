using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.NotImplemented.Extensions;

namespace Guppy.Game.Console.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterConsoleGameServices(this ContainerBuilder builder) => builder.EnsureRegisteredOnce(nameof(RegisterConsoleGameServices), builder =>
                                                                                                              {
                                                                                                                  builder.RegisterCommonGameServices().RegisterNotImplementedGraphicsServices();
                                                                                                                  builder.RegisterType<ConsoleTerminal>().AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
                                                                                                              });
    }
}