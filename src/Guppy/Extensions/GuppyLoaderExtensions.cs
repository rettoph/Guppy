﻿using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

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


            loader.Services.AddFactory<SpriteBatch>(p => new SpriteBatch(p.GetService<GraphicsDevice>()));
            loader.Services.AddFactory<PrimitiveBatch>(p => new PrimitiveBatch(p.GetService<GraphicsDevice>()));
            loader.Services.AddFactory<Camera2D>(p => new Camera2D());

            loader.Services.AddScoped<SpriteBatch>();
            loader.Services.AddScoped<PrimitiveBatch>();
            loader.Services.AddScoped<Camera2D>();

            return loader;
        }
    }
}
