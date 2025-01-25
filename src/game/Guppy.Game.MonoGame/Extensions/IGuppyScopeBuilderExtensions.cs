using Autofac;
using Guppy.Core.Commands.Common;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Extensions;
using Guppy.Game.Extensions;
using Guppy.Game.Graphics.MonoGame.Extensions;
using Guppy.Game.ImGui.MonoGame.Extensions;
using Guppy.Game.Input.Common.Messages;
using Guppy.Game.Input.Extensions;
using Guppy.Game.MonoGame.Common.Constants;
using Guppy.Game.MonoGame.Components.Engine;
using Guppy.Game.MonoGame.Components.Scene;
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

                builder.RegisterType<DrawImGuiComponent>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<EngineDebugWindowComponent>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<EngineTerminalWindowComponent>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<FpsDebugComponent>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ToggleWindowComponent>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<SceneDebugWindowComponent>().AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterType<SceneTerminalWindowComponent>().AsImplementedInterfaces().InstancePerLifetimeScope();

                builder.RegisterStateFilter<SceneDebugWindowComponent, bool>(StateKey<bool>.Create(SceneConfigurationKeys.SceneHasDebugWindow), true);
                builder.RegisterStateFilter<SceneTerminalWindowComponent, bool>(StateKey<bool>.Create(SceneConfigurationKeys.SceneHasTerminalWindow), true);

                builder.RegisterResourcePack(new ResourcePackConfiguration()
                {
                    EntryDirectory = DirectoryLocation.CurrentDirectory(GuppyMonoGamePack.Directory)
                });

                builder.RegisterInput(Inputs.ToggleDebugger, Keys.F1,
                [
                    (ButtonState.Pressed, Toggle<SceneDebugWindowComponent>.Instance)
                ]);

                builder.RegisterInput(Inputs.ToggleTerminal, Keys.OemTilde,
                [
                    (ButtonState.Pressed, Toggle<EngineTerminalWindowComponent>.Instance)
                ]);
            });
        }
    }
}