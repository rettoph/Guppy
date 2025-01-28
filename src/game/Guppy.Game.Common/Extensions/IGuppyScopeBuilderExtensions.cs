using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Logging.Common.Constants;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Extensions;
using Guppy.Game.Common.Configurations;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        /// <summary>
        /// Register a scene service filter
        /// </summary>
        /// <typeparam name="TService">The service type to be filtered</typeparam>
        /// <param name="builder"></param>
        /// <param name="sceneType">The Scene type required for the service to be valid</param>
        /// <returns></returns>
        public static IGuppyScopeBuilder RegisterSceneFilter<TService>(this IGuppyScopeBuilder builder, Type? sceneType)
            where TService : class
        {
            if (sceneType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);
            }

            return builder.RegisterStateFilter<TService, Type?>(StateKey<Type?>.Create<IScene>(), sceneType);
        }


        /// <summary>
        /// Register a scene service filter
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType">he service type to be filtered</param>
        /// <param name="sceneType">The Scene type required for the service to be valid</param>
        /// <returns></returns>
        public static IGuppyScopeBuilder RegisterSceneFilter(this IGuppyScopeBuilder builder, Type serviceType, Type? sceneType)
        {
            if (sceneType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);
            }

            return builder.RegisterStateFilter<Type?>(serviceType, StateKey<Type?>.Create<IScene>(), sceneType);
        }

        /// <summary>
        /// Register a scene service filter
        /// </summary>
        /// <typeparam name="TService">The service type to be filtered</typeparam>
        /// <typeparam name="TScene">The Scene type required for the service to be valid</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IGuppyScopeBuilder RegisterSceneFilter<TService, TScene>(this IGuppyScopeBuilder builder)
            where TService : class
            where TScene : IScene
        {
            return builder.RegisterStateFilter<TService, Type?>(StateKey<Type?>.Create<IScene>(), typeof(TScene));
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