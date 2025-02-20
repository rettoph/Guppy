using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.NotImplemented.Extensions;

namespace Guppy.Game.Console.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterConsoleGameServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterConsoleGameServices), builder =>
            {
                builder.RegisterCommonGameServices().RegisterNotImplementedGraphicsServices();
                builder.RegisterType<ConsoleTerminal>().AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
            });
        }
    }
}