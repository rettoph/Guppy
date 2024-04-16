using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Engine.Common.Autofac;

namespace Guppy.Game.Console.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterConsoleGame(this ContainerBuilder builder)
        {
            return builder.BuildOnce(nameof(RegisterConsoleGame), builder =>
            {
                builder.RegisterType<ConsoleTerminal>().AsImplementedInterfaces().AsSelf().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            });
        }
    }
}
