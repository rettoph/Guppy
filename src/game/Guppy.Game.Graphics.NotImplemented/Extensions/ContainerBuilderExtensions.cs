using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Game.Extensions;
using Guppy.Game.MonoGame.ResourceTypes;

namespace Guppy.Game.Graphics.NotImplemented.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterNotImplementedGraphicsServices(this ContainerBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterNotImplementedGraphicsServices), builder =>
            {
                builder.RegisterCommonGameServices();

                builder.RegisterType<NotImplementedEffectCodeResourceType>().As<IResourceType>().SingleInstance();
                builder.RegisterType<NotImplementedSpriteFontResourceType>().As<IResourceType>().SingleInstance();

                builder.RegisterType<NotImplementedCamera>().As<IResourceType>().SingleInstance();
                builder.RegisterType<NotImplementedContentManager>().As<IResourceType>().SingleInstance();
                builder.RegisterType<NotImplementedGameWindow>().As<IResourceType>().SingleInstance();
                builder.RegisterType<NotImplementedGraphicsDevice>().As<IResourceType>().SingleInstance();
                builder.RegisterType<NotImplementedScreen>().As<IResourceType>().SingleInstance();
            });
        }
    }
}
