using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    internal sealed class MonoGameServiceLoader : IServiceLoader
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private GameWindow _window;

        public MonoGameServiceLoader(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            _graphics = graphics;
            _content = content;
            _window = window;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_graphics);
            services.AddSingleton(_graphics.GraphicsDevice);
            services.AddSingleton(_content);
            services.AddSingleton(_window);

            services.AddScoped<SpriteBatch>();
            services.AddTransient<BasicEffect>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
