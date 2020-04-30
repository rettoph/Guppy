using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureMonoGame(this GuppyLoader loader, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            loader.Services.AddSingleton<GraphicsDeviceManager>(graphics);
            loader.Services.AddSingleton<ContentManager>(content);
            loader.Services.AddSingleton<GameWindow>(window);
            loader.Services.AddSingleton<GraphicsDevice>(graphics.GraphicsDevice);
            loader.Services.AddScoped<SpriteBatch>(p => new SpriteBatch(p.GetService<GraphicsDevice>()));
            loader.Services.AddScoped<PrimitiveBatch>(p => new PrimitiveBatch(p.GetService<GraphicsDevice>()));
            loader.Services.AddScoped<Camera2D>(p => new Camera2D());

            return loader;
        }
    }
}
