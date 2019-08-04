using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Demo.Scenes;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Demo.Layers
{
    [IsLayer]
    public class DemoLayer : Layer
    {
        private SpriteBatch _spriteBatch;

        public DemoLayer(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Entities.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            this.Entities.TryDraw(gameTime);
            _spriteBatch.End();
        }
        #endregion
    }
}
