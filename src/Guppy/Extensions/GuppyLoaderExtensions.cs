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
using Guppy.Extensions.System;
using System.Linq;
using Guppy.Extensions.System.Collections;
using Guppy.ServiceLoaders;
using Guppy.DependencyInjection;

namespace Guppy.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureMonoGame(this GuppyLoader loader, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            loader.Services.RegisterSingleton<GraphicsDeviceManager>(graphics);
            loader.Services.RegisterSingleton<ContentManager>(content);
            loader.Services.RegisterSingleton<GameWindow>(window);
            loader.Services.RegisterSingleton<GraphicsDevice>(graphics.GraphicsDevice);
            loader.RegisterServiceLoader(new MonoGameServiceLoader());

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
