using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
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
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureGame(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureGame)))
            {
                return builder;
            }

            return builder.ConfigureECS()
                .ConfigureResources()
                .ConfigureCommands()
                .AddServiceLoader(new GameLoader())
                .AddServiceLoader(new JsonLoader())
                .AddServiceLoader(new ResourceLoader())
                .AddTag(nameof(ConfigureGame));
        }

        public static GuppyConfiguration ConfigureMonoGame(
            this GuppyConfiguration builder, 
            Game game,
            GraphicsDeviceManager graphics, 
            ContentManager content, 
            GameWindow window)
        {
            if(builder.HasTag(nameof(ConfigureMonoGame)))
            {
                return builder;
            }

            return builder.ConfigureGame()
                .ConfigureInput()
                .AddServiceLoader(new MonoGameLoader(game, graphics, content, window))
                .AddTag(nameof(ConfigureMonoGame));
        }
    }
}
