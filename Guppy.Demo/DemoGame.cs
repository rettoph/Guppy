using Guppy.Demo.Configurations;
using Guppy.Demo.Entities;
using Guppy.Demo.Layers;
using Guppy.Extensions;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Demo
{
    public class DemoGame : Game
    {
        private GameWindow _window;
        private ContentManager _content;
        private GraphicsDeviceManager _graphics;

        public DemoGame(GameWindow window, ContentManager content, GraphicsDeviceManager graphics)
        {
            _window = window;
            _content = content;
            _graphics = graphics;
        }

        protected override void Boot()
        {
            base.Boot();

            this.services.AddSingleton<GameWindow>(_window);
            this.services.AddSingleton<ContentManager>(_content);
            this.services.AddSingleton<GraphicsDeviceManager>(_graphics);
            this.services.AddSingleton<GraphicsDevice>(_graphics.GraphicsDevice);
            this.services.AddSingleton<SpriteBatch>(new SpriteBatch(_graphics.GraphicsDevice));

            this.services.AddScene<BreakoutScene>();
            this.services.AddLayer<BrickLayer>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            var colorLoader = this.provider.GetLoader<ColorLoader>();
            colorLoader.Register("color:red", Color.Red);

            var stringLoader = this.provider.GetLoader<StringLoader>();
            stringLoader.Register("entity_name:brick:red", "Red Brick");
            stringLoader.Register("entity_description:brick:red", "Red Brick.");

            var contentLoader = this.provider.GetLoader<ContentLoader>();
            contentLoader.Register("texture:brick", "Sprites/brick");

            var entityLoader = this.provider.GetLoader<EntityLoader>();
            entityLoader.Register<Brick>(
                handle: "entity:brick:red",
                nameHandle: "entity_name:brick:red",
                descriptionHandle: "entity_description:brick:red",
                data: new BrickConfiguration()
                {
                    ColorHandle = "color:red",
                    Health = 1
                });
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.logger.LogInformation($"PostInitiazlizing DemoGame...");

            this.scenes.Create<BreakoutScene>();
        }
    }
}
