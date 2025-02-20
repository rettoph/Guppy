using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Common.Systems;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterSceneSystem<TSystem>(this IGuppyScopeBuilder builder)
            where TSystem : ISceneSystem
        {
            builder.RegisterScopedSystem<TSystem>();

            return builder;
        }
        public static IGuppyScopeBuilder RegisterSceneFilter(this IGuppyScopeBuilder builder, Type? sceneType, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequireScene(sceneType), build);
        }
        public static IGuppyScopeBuilder RegisterSceneFilter<TScene>(this IGuppyScopeBuilder builder, Action<IGuppyScopeBuilder> build)
            where TScene : IScene
        {
            return builder.Filter(filter => filter.RequireScene<TScene>(), build);
        }
    }
}
