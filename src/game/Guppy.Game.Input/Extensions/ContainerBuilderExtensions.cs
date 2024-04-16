using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Game.Input.Services;

namespace Guppy.Game.Input.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterGameInputServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterGameInputServices)))
            {
                return builder;
            }

            builder.RegisterType<KeyboardButtonService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MouseButtonService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CursorService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<InputService>().AsImplementedInterfaces().SingleInstance();

            return builder.AddTag(nameof(RegisterGameInputServices));
            // return builder.RegisterServiceLoader<MonoGameLoader>().AddTag(nameof(RegisterMonoGame));
        }
    }
}
