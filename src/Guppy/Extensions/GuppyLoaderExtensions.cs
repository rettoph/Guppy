using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Guppy.ServiceLoaders;
using Guppy.DependencyInjection;

namespace Guppy.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureMonoGame(this GuppyLoader loader, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            loader.Services.RegisterTypeFactory<GraphicsDeviceManager>(p => graphics);
            loader.Services.RegisterTypeFactory<ContentManager>(p => content);
            loader.Services.RegisterTypeFactory<GameWindow>(p => window);
            loader.Services.RegisterTypeFactory<GraphicsDevice>(p => graphics.GraphicsDevice);

            loader.Services.RegisterService<GraphicsDeviceManager>().SetLifetime(ServiceLifetime.Singleton);
            loader.Services.RegisterService<ContentManager>().SetLifetime(ServiceLifetime.Singleton);
            loader.Services.RegisterService<GameWindow>().SetLifetime(ServiceLifetime.Singleton);
            loader.Services.RegisterService<GraphicsDevice>().SetLifetime(ServiceLifetime.Singleton);

            loader.RegisterServiceLoader(new MonoGameServiceLoader());
            loader.RegisterServiceLoader(new ContentServiceLoader());

            return loader;
        }

        public static T BuildGame<T>(this GuppyLoader guppy, ServiceConfigurationKey? key = null)
            where T : Game
        {
            if (!guppy.Initialized)
                throw new Exception("Please initialize Guppy before building a game instance.");

            return guppy.BuildServiceProvider().GetService<T>(key ?? ServiceConfigurationKey.From<T>());
        }
    }
}
