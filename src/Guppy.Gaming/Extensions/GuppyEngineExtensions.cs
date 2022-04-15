using Guppy.Gaming;
using Guppy.Gaming.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureGame<TGame>(
            this GuppyEngine guppy)
                where TGame : Gaming.Game
        {
            if(guppy.Tags.Contains(nameof(ConfigureGame)))
            {
                return guppy;
            }

            return guppy.AddLoader(new GamingServiceLoader<TGame>())
                .AddTag(nameof(ConfigureGame));
        }

        public static GuppyEngine ConfigureMonoGame<TGame>(
            this GuppyEngine guppy, 
            GraphicsDeviceManager graphics, 
            ContentManager content,
            GameWindow window)
                where TGame : Gaming.Game
        {
            if (guppy.Tags.Contains(nameof(ConfigureMonoGame)))
            {
                return guppy;
            }

            return guppy.ConfigureGame<TGame>()
                .AddLoader(new MonoGameServiceLoader(graphics, content, window))
                .AddTag(nameof(ConfigureMonoGame));
        }
    }
}
