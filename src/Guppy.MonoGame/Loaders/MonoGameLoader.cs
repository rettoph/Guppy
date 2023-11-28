using Autofac;
using Guppy.MonoGame;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Guppy.Attributes;
using Guppy.MonoGame.Components;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class MonoGameLoader : IServiceLoader
    {
        private Microsoft.Xna.Framework.Game _game;
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private GameWindow _window;

        public MonoGameLoader(Microsoft.Xna.Framework.Game game, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            _game = game;
            _graphics = graphics;
            _content = content;
            _window = window;
        }

        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Screen>().As<IScreen>().InstancePerLifetimeScope();
            services.RegisterInstance(_game).SingleInstance();
            services.RegisterInstance<GraphicsDeviceManager>(_graphics).SingleInstance();
            services.RegisterInstance<GraphicsDevice>(_graphics.GraphicsDevice).SingleInstance();
            services.RegisterInstance<ContentManager>(_content).SingleInstance();
            services.RegisterInstance<GameWindow>(_window).SingleInstance();
            services.RegisterType<SpriteBatch>().SingleInstance();

            services.RegisterType<Terminal>().As<ITerminal>().AsSelf().InstancePerLifetimeScope();

            services.RegisterGeneric(typeof(PrimitiveBatch<,>));
            services.RegisterGeneric(typeof(PrimitiveBatch<>));

            services.AddInput(Inputs.ToggleDebugger, Keys.F1, new[]
            {
                (ButtonState.Pressed, Toggle<DebugWindowComponent>.Instance)
            });

            services.AddInput(Inputs.ToggleTerminal, Keys.OemTilde, new[]
{
                (ButtonState.Pressed, Toggle<TerminalWindowComponent>.Instance)
            });
        }
    }
}
