using Guppy.Common.DependencyInjection;
using Guppy.Common.Providers;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.MonoGame.Primitives;
using Guppy.MonoGame.Services;
using Guppy.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Game>(_game);
            services.AddSingleton<GraphicsDeviceManager>(_graphics);
            services.AddSingleton<GraphicsDevice>(_graphics.GraphicsDevice);
            services.AddSingleton<ContentManager>(_content);
            services.AddSingleton<GameWindow>(_window);

            // services.AddTransient<IResourcePackTypeProvider, ResourcePackContentProvider<Texture2D>>();
            // services.AddTransient<IResourcePackTypeProvider, ResourcePackContentProvider<SpriteFont>>();

            services.AddInput(InputConstants.ToggleTerminal, Keys.OemTilde, new[]
            {
                (ButtonState.Pressed, Toggle<ITerminalService>.Instance)
            });

            services.ConfigureCollection(manager =>
            {
                manager.GetService<InputService>()
                    .SetLifetime(ServiceLifetime.Scoped)
                    .AddAlias<IInputService>()
                    .AddAlias<IGameComponent>();
            });

            // Add descriptor for some primitive batch cameras
            var vertexTypes = typeof(Game).Assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(IVertexType)) && x.IsValueType);
            foreach (Type vertexType in vertexTypes)
            {
                var primitiveBatchType = typeof(PrimitiveBatch<>).MakeGenericType(vertexType);
                services.AddSingleton(primitiveBatchType);
            }
        }
    }
}
