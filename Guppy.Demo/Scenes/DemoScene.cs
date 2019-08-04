using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Demo.Entities;
using Guppy.Demo.Layers;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Demo.Scenes
{
    [IsScene]
    public class DemoScene : Scene
    {
        private GraphicsDevice _graphics;
        public DemoScene(GraphicsDevice graphics)
        {
            _graphics = graphics;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var layer = this.layers.Build<DemoLayer>(0);

            var e1 = this.entities.Build<DemoEntity>("entity:demo");
            this.entities.Build<DemoEntity>("entity:demo");
            this.entities.Build<DemoEntity>("entity:demo");
            this.entities.Build<DemoEntity>("entity:demo");
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.DarkRed);

            base.Draw(gameTime);
        }
    }
}
