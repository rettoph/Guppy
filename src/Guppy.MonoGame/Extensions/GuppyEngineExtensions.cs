using Guppy.MonoGame.Initializers;
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
            if (guppy.Tags.Contains(nameof(ConfigureGame)))
            {
                return guppy;
            }

            return guppy.ConfigureECS()
                .ConfigureResources()
                .AddInitializer(new CommandInitializer(), 0)
                .AddLoader(new GameLoader<TGlobalBrokerPublishStrategy>(), 0)
                .AddLoader(new JsonLoader(), 0)
                .AddLoader(new ResourceLoader(), 0)
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
            if(guppy.Tags.Contains(nameof(ConfigureMonoGame)))
            {
                return guppy;
            }

            return guppy.ConfigureGame<TGlobalBrokerPublishStrategy>()
                .AddInitializer(new InputInitializer(), 0)
                .AddLoader(new MonoGameLoader(game, graphics, content, window), 0)
                .AddTag(nameof(ConfigureMonoGame));
        }
    }
}
