using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Game.Common;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.MonoGame.Systems;
using Guppy.Game.ImGui.MonoGame.Extensions;
using Guppy.Game.Input.Extensions;
using Guppy.Game.MonoGame.Common.Utilities.Cameras;
using Guppy.Game.MonoGame.ResourceTypes;
using Guppy.Game.MonoGame.Systems.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.Graphics.MonoGame.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterMonoGameGraphicsService(
            this IGuppyRootBuilder builder,
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

                builder.RegisterResourceType<MonoGameEffectCodeResourceType>();
                builder.RegisterResourceType<MonoGameSpriteFontResourceType>();

                builder.RegisterGlobalSystem<MonoGameWorldViewProjectionEffectSystem>();

                builder.RegisterSceneFilter<IScene>(builder =>
                {
                    builder.RegisterSceneSystem<MonoGameScreenSystem>();
                });
            });
        }
    }
}