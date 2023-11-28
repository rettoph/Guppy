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
        private Game _game;
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private GameWindow _window;

        public MonoGameLoader(Game game, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            _game = game;
            _graphics = graphics;
            _content = content;
            _window = window;
        }

        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Screen>().As<IScreen>().InstancePerLifetimeScope();
            services.RegisterInstance<Game>(_game).SingleInstance();
            services.RegisterInstance<GraphicsDeviceManager>(_graphics).SingleInstance();
            services.RegisterInstance<GraphicsDevice>(_graphics.GraphicsDevice).SingleInstance();
            services.RegisterInstance<ContentManager>(_content).SingleInstance();
            services.RegisterInstance<GameWindow>(_window).SingleInstance();
            services.RegisterType<SpriteBatch>().SingleInstance();

            services.RegisterType<Terminal>().As<ITerminal>().AsSelf().SingleInstance();

            services.RegisterGeneric(typeof(PrimitiveBatch<,>));
            services.RegisterGeneric(typeof(PrimitiveBatch<>));

            services.AddInput(Inputs.ToggleDebugger, Keys.F1, new[]
            {
                (ButtonState.Pressed, Toggle<DebugWindowComponent>.Instance)
            });
        }
    }
}
