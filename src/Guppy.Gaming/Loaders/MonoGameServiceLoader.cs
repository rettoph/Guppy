using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Loaders
{
    internal sealed class MonoGameServiceLoader : IServiceLoader
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ContentManager _content;
        private readonly GameWindow _window;

        public MonoGameServiceLoader(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            _graphics = graphics;
            _content = content;
            _window = window;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GraphicsDeviceManager>(_graphics);
            services.AddSingleton<GraphicsDevice>(_graphics.GraphicsDevice);
            services.AddSingleton<ContentManager>(_content);
            services.AddSingleton<GameWindow>(_window);
            services.AddScoped<SpriteBatch>(p => new SpriteBatch(_graphics.GraphicsDevice));
        }
    }
}
