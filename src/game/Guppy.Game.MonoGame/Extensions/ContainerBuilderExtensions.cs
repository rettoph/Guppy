using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Extensions.Autofac;
using Guppy.Engine.Common.Autofac;
using Guppy.Game.Extensions;
using Guppy.Game.Input.Extensions;
using Guppy.Game.MonoGame.Common;
using Guppy.Game.MonoGame.Common.Constants;
using Guppy.Game.MonoGame.Common.Primitives;
using Guppy.Game.MonoGame.Components.Guppy;
using Guppy.Game.MonoGame.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.MonoGame.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterMonoGameServices(
            this ContainerBuilder builder,
            Microsoft.Xna.Framework.Game game,
            GraphicsDeviceManager graphics,
            ContentManager content,
            GameWindow window)
        {
            return builder.BuildOnce(nameof(RegisterMonoGameServices), builder =>
            {
                builder.RegisterCommonGameServices().RegisterGameInputServices();

                builder.RegisterInstance(game).SingleInstance();
                builder.RegisterInstance<GraphicsDeviceManager>(graphics).SingleInstance();
                builder.RegisterInstance<GraphicsDevice>(graphics.GraphicsDevice).SingleInstance();
                builder.RegisterInstance<ContentManager>(content).SingleInstance();
                builder.RegisterInstance<GameWindow>(window).SingleInstance();

                builder.RegisterType<Screen>().As<IScreen>().InstancePerLifetimeScope();
                builder.RegisterType<SpriteBatch>().SingleInstance();

                builder.RegisterType<MonoGameTerminal>().AsImplementedInterfaces().AsSelf().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);

                builder.RegisterGeneric(typeof(PrimitiveBatch<,>));
                builder.RegisterGeneric(typeof(PrimitiveBatch<>));
                builder.RegisterGeneric(typeof(StaticPrimitiveBatch<,>));
                builder.RegisterGeneric(typeof(StaticPrimitiveBatch<>));

                builder.RegisterResourcePack(new ResourcePackConfiguration()
                {
                    EntryDirectory = DirectoryLocation.CurrentDirectory(GuppyMonoGamePack.Directory)
                });

                builder.RegisterInput(Inputs.ToggleDebugger, Keys.F1, new[]
                {
                    (ButtonState.Pressed, Toggle<GuppyDebugWindowComponent>.Instance)
                });

                builder.RegisterInput(Inputs.ToggleTerminal, Keys.OemTilde, new[]
                {
                    (ButtonState.Pressed, Toggle<TerminalWindowComponent>.Instance)
                });
            });
        }
    }
}
