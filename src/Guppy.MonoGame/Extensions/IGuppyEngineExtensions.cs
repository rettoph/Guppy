using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.MonoGame.Loaders;
using Guppy.MonoGame.Strategies.PublishStrategies;
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
        public static GuppyEngine ConfigureGame<TGlobalBrokerPublishStrategy>(this GuppyEngine guppy)
            where TGlobalBrokerPublishStrategy : PublishStrategy
        {
            if (guppy.HasTag(nameof(ConfigureGame)))
            {
                return guppy;
            }

            return guppy.ConfigureECS()
                .ConfigureResources()
                .AddServiceLoader(new GameLoader<TGlobalBrokerPublishStrategy>())
                .AddServiceLoader(new JsonLoader())
                .AddServiceLoader(new ResourceLoader())
                .AddTag(nameof(ConfigureGame));
        }

        public static GuppyEngine ConfigureMonoGame<TGlobalBrokerPublishStrategy>(
            this GuppyEngine guppy, 
            Game game,
            GraphicsDeviceManager graphics, 
            ContentManager content, 
            GameWindow window)
                where TGlobalBrokerPublishStrategy : PublishStrategy
        {
            if(guppy.HasTag(nameof(ConfigureMonoGame)))
            {
                return guppy;
            }

            return guppy.ConfigureGame<TGlobalBrokerPublishStrategy>()
                .AddServiceLoader(new MonoGameLoader(game, graphics, content, window))
                .AddTag(nameof(ConfigureMonoGame));
        }
    }
}
