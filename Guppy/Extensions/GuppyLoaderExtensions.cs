using Guppy.ServiceLoaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureMonoGame(this GuppyLoader loader, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            loader.AddServiceLoader(new MonoGameServiceLoader(graphics, content, window));

            return loader;
        }
    }
}
