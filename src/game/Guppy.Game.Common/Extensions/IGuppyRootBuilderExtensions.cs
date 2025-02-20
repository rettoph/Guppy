using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Common.Systems;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterSceneSystem<TSystem>(this IGuppyRootBuilder builder)
            where TSystem : ISceneSystem
        {
            builder.RegisterScopedSystem<TSystem>();

            return builder;
        }
        public static IGuppyRootBuilder RegisterSceneFilter(this IGuppyRootBuilder builder, Type? sceneType, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequireScene(sceneType), build);
        }

        public static IGuppyRootBuilder RegisterSceneFilter<TScene>(this IGuppyRootBuilder builder, Action<IGuppyScopeBuilder> build)
            where TScene : IScene
        {
            return builder.Filter(filter => filter.RequireScene<TScene>(), build);
        }
    }
}
