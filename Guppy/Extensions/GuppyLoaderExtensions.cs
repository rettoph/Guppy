using Guppy.Extensions.DependencyInjection;
using Guppy.Utilities.Cameras;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    /// <summary>
    /// Contains default guppy loader extensions 
    /// used to add additional functionality to
    /// the guppy loader pre initialization
    /// </summary>
    public static class GuppyLoaderExtensions
    {
        /// <summary>
        /// Automatically register useful MonoGame services directly into the internal
        /// GuppyLoader service collection.
        /// </summary>
        /// <param name="guppy"></param>
        /// <param name="graphics"></param>
        /// <param name="window"></param>
        /// <param name="content"></param>
        public static void ConfigureMonoGame(this GuppyLoader guppy, GraphicsDeviceManager graphics, GameWindow window, ContentManager content)
        {
            if (guppy.Initialized)
                throw new Exception("Unable to add services to Guppy! GuppyLoader instance has already been initialized.");

            guppy.Services.AddSingleton(graphics);
            guppy.Services.AddSingleton(window);
            guppy.Services.AddSingleton(content);
            guppy.Services.AddSingleton(graphics.GraphicsDevice);
            guppy.Services.AddScoped<SpriteBatch>();

            guppy.Services.AddPooledTransient<Camera2D>();
            guppy.Services.AddTransient<BasicEffect>();
        }
    }
}
