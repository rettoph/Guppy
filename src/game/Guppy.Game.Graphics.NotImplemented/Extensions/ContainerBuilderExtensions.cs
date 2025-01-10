using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Resources.Common.Extensions.Autofac;
using Guppy.Game.Extensions;
using Guppy.Game.MonoGame.ResourceTypes;

namespace Guppy.Game.Graphics.NotImplemented.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterNotImplementedGraphicsServices(this ContainerBuilder builder) => builder.EnsureRegisteredOnce(nameof(RegisterNotImplementedGraphicsServices), builder =>
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