using Autofac;
using Autofac.Core;
using Guppy.Core.Common;
using Guppy.Engine.Common.Loaders;

namespace Guppy.Engine.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Create an instance of said service loader and invoke onto the given builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterModule(this ContainerBuilder builder, Type type, IContainer? boot = null)
        {
            ThrowIf.Type.IsNotAssignableFrom<IModule>(type);

            IModule instance = boot is null ? (IModule?)Activator.CreateInstance(type) ?? throw new Exception() : (IModule)boot.Resolve(type);
            builder.RegisterModule(instance);

            return builder;
        }

        /// <summary>
        /// Create an instance of said service loader and invoke onto the given builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterServiceLoader(this ContainerBuilder builder, Type type, IContainer? boot = null)
        {
            ThrowIf.Type.IsNotAssignableFrom<IServiceLoader>(type);

            IServiceLoader instance = boot is null ? (IServiceLoader?)Activator.CreateInstance(type) ?? throw new Exception() : (IServiceLoader)boot.Resolve(type);
            return builder.RegisterServiceLoader(instance);
        }

        /// <summary>
        /// Create an instance of said service loader and invoke onto the given builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterServiceLoader<T>(this ContainerBuilder builder, IContainer? boot = null)
            where T : IServiceLoader
        {
            IServiceLoader instance = boot is null ? Activator.CreateInstance<T>() : (IServiceLoader)boot.Resolve<T>();
            return builder.RegisterServiceLoader(instance);
        }

        /// <summary>
        /// Run an instance of said service loader and invoke onto the given builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterServiceLoader(this ContainerBuilder builder, IServiceLoader instance)
        {
            instance.ConfigureServices(builder);

            return builder;
        }
    }
}
