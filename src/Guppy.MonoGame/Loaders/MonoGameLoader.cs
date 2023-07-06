using Autofac;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.MonoGame.Primitives;
using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            services.RegisterInstance<Game>(_game).SingleInstance();
            services.RegisterInstance<GraphicsDeviceManager>(_graphics).SingleInstance();
            services.RegisterInstance<GraphicsDevice>(_graphics.GraphicsDevice).SingleInstance();
            services.RegisterInstance<ContentManager>(_content).SingleInstance();
            services.RegisterInstance<GameWindow>(_window).SingleInstance();

            // services.AddTransient<IResourcePackTypeProvider, ResourcePackContentProvider<Texture2D>>();
            // services.AddTransient<IResourcePackTypeProvider, ResourcePackContentProvider<SpriteFont>>();

            services.AddInput(InputConstants.ToggleTerminal, Keys.OemTilde, new[]
            {
                (true, Toggle<ITerminalService>.Instance)
            });

            // Add descriptor for some primitive batch cameras
            var vertexTypes = typeof(Game).Assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(IVertexType)) && x.IsValueType);
            foreach (Type vertexType in vertexTypes)
            {
                var primitiveBatchType = typeof(PrimitiveBatch<>).MakeGenericType(vertexType);
                services.RegisterType(primitiveBatchType);
            }
        }
    }
}
