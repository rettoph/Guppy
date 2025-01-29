using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Common.Constants;
using Guppy.Game.Common.Configurations;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterSceneFilter(this IGuppyScopeBuilder builder, Type? sceneType, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequireScene(sceneType), build);
        }

        public static IGuppyScopeBuilder RegisterSceneFilter<TScene>(this IGuppyScopeBuilder builder, Action<IGuppyScopeBuilder> build)
            where TScene : IScene
        {
            return builder.Filter(filter => filter.RequireScene<TScene>(), build);
        }

        public static IGuppyScopeBuilder ConfigureTerminalLogMessageSink(
            this IGuppyScopeBuilder builder,
            string outputTemplate = LoggingConstants.DefaultOutputTemplate,
            bool enabled = true)
        {
            return builder.Configure<TerminalLogMessageSinkConfiguration>(conf =>
            {
                conf.OutputTemplate = outputTemplate;
                conf.Enabled = enabled;
            });
        }

        public static IGuppyScopeBuilder ConfigureTerminalLogMessageSink(
            this IGuppyScopeBuilder builder,
            Action<IGuppyScope, TerminalLogMessageSinkConfiguration> configurator)
        {
            return builder.Configure<TerminalLogMessageSinkConfiguration>(configurator);
        }
    }
}