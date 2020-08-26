using Guppy.DependencyInjection;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.UI.Utilities.Backgrounds
{
    public class ImageBackground : Background
    {
        private SpriteBatch _spriteBatch;
        private IElement _element;

        public Texture2D Texture { get; set; }

        public override void Setup(ServiceProvider provider, IElement element)
        {
            base.Setup(provider, element);

            _element = element;
            provider.Service(out _spriteBatch);
        }

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(this.Texture, _element.Bounds.Pixel, Color.White);
        }
        #endregion
    }
}
