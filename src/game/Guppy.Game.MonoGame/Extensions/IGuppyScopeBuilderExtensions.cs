using Autofac;
using Guppy.Core.Commands.Common;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Game.Common;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.MonoGame.Extensions;
using Guppy.Game.ImGui.MonoGame.Extensions;
using Guppy.Game.Input.Common.Messages;
using Guppy.Game.Input.Extensions;
using Guppy.Game.MonoGame.Common.Constants;
using Guppy.Game.MonoGame.Common.Extensions;
using Guppy.Game.MonoGame.Systems.Engine;
using Guppy.Game.MonoGame.Systems.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.MonoGame.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterMonoGameServices(
            this IGuppyScopeBuilder builder,
            Microsoft.Xna.Framework.Game game,
            GraphicsDeviceManager graphics,
            ContentManager content,
            GameWindow window)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterMonoGameServices), builder =>
            {
                builder.RegisterCommonGameServices()
                    .RegisterMonoGameGraphicsService(game, graphics, content, window)
                    .RegisterGameMonoGameImGuiServices()
                    .RegisterGameInputServices();

                builder.RegisterType<MonoGameTerminal>().AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();

                builder.RegisterGlobalSystem<DrawImGuiSystem>();
                builder.RegisterGlobalSystem<EngineDebugWindowSystem>();
                builder.RegisterGlobalSystem<EngineTerminalWindowSystem>();
                builder.RegisterGlobalSystem<FpsDebugSystem>();
                builder.RegisterGlobalSystem<ToggleWindowSystem>();

                builder.Filter(
                    filter => filter.RequireScene<IScene>().RequireSceneHasDebugWindow(true),
                    builder =>
                    {
                        builder.RegisterSceneSystem<SceneDebugWindowSystem>();
                    });

                builder.Filter(
                    filter => filter.RequireScene<IScene>().RequireSceneHasTerminalWindow(true),
                    builder =>
                    {
                        builder.RegisterSceneSystem<SceneTerminalWindowSystem>();
                    });

                builder.RegisterResourcePack(new ResourcePackConfiguration()
                {
                    EntryDirectory = DirectoryLocation.CurrentDirectory(GuppyMonoGamePack.Directory)
                });

                builder.RegisterInput(Inputs.ToggleDebugger, Keys.F1,
                [
                    (ButtonState.Pressed, Toggle<SceneDebugWindowSystem>.Instance)
                ]);

                builder.RegisterInput(Inputs.ToggleTerminal, Keys.OemTilde,
                [
                    (ButtonState.Pressed, Toggle<EngineTerminalWindowSystem>.Instance)
                ]);
            });
        }
    }
}