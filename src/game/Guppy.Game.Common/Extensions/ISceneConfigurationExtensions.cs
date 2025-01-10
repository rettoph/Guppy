using Autofac;

namespace Guppy.Game.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        private const string _containerBuilder = nameof(_containerBuilder);

        public static ISceneConfiguration WithContainerBuilder(this ISceneConfiguration configuration, Action<ContainerBuilder> builder)
        {
            return configuration.Set(_containerBuilder, builder);
        }

        public static Action<ContainerBuilder>? GetContainerBuilder(this ISceneConfiguration configuration)
        {
            return configuration.Get<Action<ContainerBuilder>>(_containerBuilder);
        }
    }
}