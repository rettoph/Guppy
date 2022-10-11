using Guppy.Loaders;
using Guppy.MonoGame.Commands;
using Guppy.MonoGame.Providers.ResourcePackTypeProviders;
using Guppy.MonoGame.Systems;
using Guppy.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

            services.AddTransient<IResourcePackTypeProvider, ResourcePackContentProvider<Texture2D>>();
            services.AddTransient<IResourcePackTypeProvider, ResourcePackContentProvider<SpriteFont>>();

            services.AddSystem<InputSystem>(0);

            services.AddInput("toggle_terminal", Keys.OemTilde, new[]
            {
                (ButtonState.Pressed, new ToggleWindow() { Window = ToggleWindow.Windows.Terminal })
            });

            services.AddInput("toggle_debugger", Keys.F1, new[]
            {
                (ButtonState.Pressed, new ToggleWindow() { Window = ToggleWindow.Windows.Debugger })
            });
        }
    }
}
