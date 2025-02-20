using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Input.Services;
using Guppy.Game.Input.Systems;

namespace Guppy.Game.Input.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterGameInputServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterGameInputServices), builder =>
            {
                builder.RegisterType<KeyboardButtonService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<MouseButtonService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<CursorService>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<InputService>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterGlobalSystem<ButtonPublishSystem>();
                builder.RegisterGlobalSystem<MouseCursorPublishSystem>();
            });
        }
    }
}