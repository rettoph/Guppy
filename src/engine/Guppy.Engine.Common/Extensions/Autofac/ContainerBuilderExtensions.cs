using Autofac;
using Autofac.Core;
using Guppy.Core.Common;

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
            if (type.IsAbstract == true)
            { // No need to register abstract modules
                return builder;
            }

            IModule instance = boot is null ? (IModule?)Activator.CreateInstance(type) ?? throw new Exception() : (IModule)boot.Resolve(type);
            builder.RegisterModule(instance);

            return builder;
        }
    }
}