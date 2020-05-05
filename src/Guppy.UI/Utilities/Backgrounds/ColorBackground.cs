using Guppy.DependencyInjection;
using Guppy.Extensions.Utilities;
using Guppy.UI.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Backgrounds
{
    public class ColorBackground : Background
    {
        private IElement _element;
        private Texture2D _pixel;

        public Color Color { get; set; }

        public override void Setup(ServiceProvider provider, IElement element)
        {
            base.Setup(provider, element);

            _element = element;
            _pixel = provider.GetService<GraphicsHelper>().Pixel;
        }

        public override void Draw(GameTime gameTime)
        {
            _element.SpriteBatch.Draw(_pixel, _element.Bounds.Pixel, this.Color);
        }
    }
}
