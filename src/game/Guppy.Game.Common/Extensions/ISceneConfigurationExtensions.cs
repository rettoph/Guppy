using Autofac;

namespace Guppy.Game.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        private const string _containerBuilder = nameof(_containerBuilder);

        public static ISceneConfiguration WithContainerBuilder(this ISceneConfiguration configuration, Action<ContainerBuilder> builder) => configuration.Set(_containerBuilder, builder);

        public static Action<ContainerBuilder>? GetContainerBuilder(this ISceneConfiguration configuration) => configuration.Get<Action<ContainerBuilder>>(_containerBuilder);
    }
}