using Autofac;

namespace Guppy.Game.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        private const string ContainerBuilder = nameof(ContainerBuilder);

        public static ISceneConfiguration WithContainerBuilder(this ISceneConfiguration configuration, Action<ContainerBuilder> builder)
        {
            return configuration.Set(ContainerBuilder, builder);
        }

        public static Action<ContainerBuilder>? GetContainerBuilder(this ISceneConfiguration configuration)
        {
            return configuration.Get<Action<ContainerBuilder>>(ContainerBuilder);
        }
    }
}
