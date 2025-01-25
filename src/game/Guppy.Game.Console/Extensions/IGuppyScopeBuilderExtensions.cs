using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.NotImplemented.Extensions;

namespace Guppy.Game.Console.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterConsoleGameServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterConsoleGameServices), builder =>
            {
                builder.RegisterCommonGameServices().RegisterNotImplementedGraphicsServices();
                builder.RegisterType<ConsoleTerminal>().AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
            });
        }
    }
}