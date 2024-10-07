using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.Common;
using Guppy.Game.ImGui.MonoGame.Extensions;
using Guppy.Game.Input.Extensions;
using Guppy.Game.MonoGame.Common.Utilities.Cameras;
using Guppy.Game.MonoGame.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.Graphics.MonoGame.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterMonoGameGraphicsService(
            this ContainerBuilder builder,
            Microsoft.Xna.Framework.Game game,
            GraphicsDeviceManager graphics,
            ContentManager content,
            GameWindow window)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterMonoGameGraphicsService), builder =>
            {
                builder.RegisterCommonGameServices().RegisterGameMonoGameImGuiServices().RegisterGameInputServices();

                builder.RegisterInstance(game).SingleInstance();
                builder.RegisterInstance<GraphicsDeviceManager>(graphics).SingleInstance();
                builder.RegisterInstance<GraphicsDevice>(graphics.GraphicsDevice).SingleInstance();
                builder.RegisterInstance<ContentManager>(content).SingleInstance();
                builder.RegisterInstance<GameWindow>(window).SingleInstance();

                builder.RegisterType<MonoGameGraphicsDevice>().As<IGraphicsDevice>().SingleInstance();
                builder.RegisterType<MonoGameGameWindow>().As<IGameWindow>().SingleInstance();
                builder.RegisterType<MonoGameContentManager>().As<IContentManager>().SingleInstance();

                builder.RegisterType<MonoGameCamera2D>().As<ICamera2D>().SingleInstance();

                builder.RegisterType<MonoGameScreen>().As<IScreen>().InstancePerLifetimeScope();
                builder.RegisterType<SpriteBatch>().SingleInstance();

                builder.RegisterType<MonoGameEffectCodeResourceType>().As<IResourceType>().SingleInstance();
                builder.RegisterType<MonoGameSpriteFontResourceType>().As<IResourceType>().SingleInstance();
            });
        }
    }
}
