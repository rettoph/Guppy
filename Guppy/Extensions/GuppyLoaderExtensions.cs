using Guppy.ServiceLoaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        public static GuppyLoader ConfigureMonoGame(this GuppyLoader guppy, GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            guppy.AddServiceLoader(new MonoGameConfigurationServiceLoader(graphics, content, window));

            return guppy;
        }
    }
}
