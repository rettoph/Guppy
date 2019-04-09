using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements.ElementSegments
{
    public class InnerElementSegment : ElementSegment
    {
        public InnerElementSegment(Element element) : base(element)
        {
        }

        public override Color getDebugVerticeColor(ElementState state)
        {
            return this.element.Style.Get<Color>(state, StyleProperty.InnerDebugBoudaryColor);
        }

        public override void generateTexture(ElementState state, SpriteBatch spriteBatch)
        {
        }
    }
}
