using Guppy.Attributes;
using Guppy.Demo.Scenes;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Guppy.Demo
{
    [IsGame]
    public class DemoGuppyGame : Guppy.Game
    {
        private DemoScene _scene;

        protected override void Initialize()
        {
            base.Initialize();

            _scene = this.provider.GetScene<DemoScene>();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _scene.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _scene.TryUpdate(gameTime);
        }
    }
}
