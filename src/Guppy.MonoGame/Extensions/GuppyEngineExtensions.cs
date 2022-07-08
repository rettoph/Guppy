using Guppy.MonoGame.Initializers;
using Guppy.MonoGame.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureGame(this GuppyEngine guppy)
        {
            if (guppy.Tags.Contains(nameof(ConfigureGame)))
            {
                return guppy;
            }

            return guppy.ConfigureECS()
                .ConfigureResources()
                .AddInitializer(new CommandInitializer())
                .AddLoader(new JsonLoader())
                .AddLoader(new ResourceLoader())
                .AddTag(nameof(ConfigureGame));
        }

        public static GuppyEngine ConfigureMonoGame(
            this GuppyEngine guppy, 
            Game game,
            GraphicsDeviceManager graphics, 
            ContentManager content, 
            GameWindow window)
        {
            if(guppy.Tags.Contains(nameof(ConfigureMonoGame)))
            {
                return guppy;
            }

            return guppy.ConfigureGame()
                .AddInitializer(new InputInitializer())
                .AddLoader(new MonoGameLoader(game, graphics, content, window))
                .AddTag(nameof(ConfigureMonoGame));
        }
    }
}
