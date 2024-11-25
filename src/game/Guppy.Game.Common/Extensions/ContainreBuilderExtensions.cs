using Autofac;
using Guppy.Core.Common;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Extensions;

namespace Guppy.Game.Common.Extensions
{
    public static class ContainreBuilderExtensions
    {
        /// <summary>
        /// Register a scene service filter
        /// </summary>
        /// <typeparam name="TService">The service type to be filtered</typeparam>
        /// <param name="builder"></param>
        /// <param name="sceneType">The Scene type required for the service to be valid</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterSceneFilter<TService>(this ContainerBuilder builder, Type? sceneType)
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
        public static ContainerBuilder RegisterSceneFilter(this ContainerBuilder builder, Type serviceType, Type? sceneType)
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
        public static ContainerBuilder RegisterSceneFilter<TService, TScene>(this ContainerBuilder builder)
            where TService : class
            where TScene : IScene
        {
            return builder.RegisterStateFilter<TService, Type?>(StateKey<Type?>.Create<IScene>(), typeof(TScene));
        }
    }
}
