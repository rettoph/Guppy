using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Internal service loader added within the ConfigureMonoGame extension method.
    /// 
    /// This service loader will not be ran unless that extension method is called.
    /// </summary>
    internal class MonoGameConfigurationServiceLoader : IServiceLoader
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private GameWindow _window;

        public MonoGameConfigurationServiceLoader(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
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
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
