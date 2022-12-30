using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages.Inputs;
using Guppy.MonoGame.Providers.ResourcePackTypeProviders;
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

            services.AddInput(InputConstants.ToggleTerminal, Keys.OemTilde, new[]
            {
                (ButtonState.Pressed, new ToggleWindowInput() { Window = ToggleWindowInput.Windows.Terminal })
            });

            services.AddInput(InputConstants.ToggleDebugger, Keys.F1, new[]
            {
                (ButtonState.Pressed, new ToggleWindowInput() { Window = ToggleWindowInput.Windows.Debugger })
            });
        }
    }
}
