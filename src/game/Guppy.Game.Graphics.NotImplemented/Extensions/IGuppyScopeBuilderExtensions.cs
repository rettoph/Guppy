using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Game.Extensions;
using Guppy.Game.MonoGame.ResourceTypes;

namespace Guppy.Game.Graphics.NotImplemented.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterNotImplementedGraphicsServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterNotImplementedGraphicsServices), builder =>
            {
                builder.RegisterCommonGameServices();

                builder.RegisterResourceType<NotImplementedEffectCodeResourceType>();
                builder.RegisterResourceType<NotImplementedSpriteFontResourceType>();

                builder.RegisterType<NotImplementedCamera>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<NotImplementedContentManager>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<NotImplementedGameWindow>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<NotImplementedGraphicsDevice>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<NotImplementedScreen>().AsImplementedInterfaces().SingleInstance();
            });
        }
    }
}