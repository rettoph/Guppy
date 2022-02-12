using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy;
using Guppy.Interfaces;
using System.Linq;
using Guppy.ServiceLoaders;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureMonoGame(this GuppyLoader loader, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            loader.Services.RegisterService<GraphicsDeviceManager>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory => factory.SetMethod(_ => graphics));

            loader.Services.RegisterService<ContentManager>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory => factory.SetMethod(_ => content));

            loader.Services.RegisterService<GameWindow>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory => factory.SetMethod(_ => window));

            loader.Services.RegisterService<GraphicsDevice>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory => factory.SetMethod(_ => graphics.GraphicsDevice));

            loader.RegisterServiceLoader(new MonoGameServiceLoader());
            loader.RegisterServiceLoader(new ContentServiceLoader());

            return loader;
        }

        public static T BuildGame<T>(this GuppyLoader guppy, Type type = null)
            where T : Game
        {
            if (!guppy.Initialized)
                throw new Exception("Please initialize Guppy before building a game instance.");

            return guppy.BuildServiceProvider().GetService<T>(type ?? typeof(T));
        }
    }
}
