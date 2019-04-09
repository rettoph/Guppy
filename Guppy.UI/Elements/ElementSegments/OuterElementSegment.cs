using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements.ElementSegments
{
    public class OuterElementSegment : ElementSegment
    {
        public OuterElementSegment(Element element) : base(element)
        {
        }

        public override Color getDebugVerticeColor(ElementState state)
        {
            return this.element.Style.Get<Color>(state, StyleProperty.OuterDebugBoundaryColor);
        }

        public override void generateTexture(ElementState state, SpriteBatch spriteBatch)
        {
            var texture = this.element.Style.Get<Texture2D>(state, StyleProperty.BackgroundImage);

            if (texture != null) {
                switch (this.element.Style.Get<DrawType>(state, StyleProperty.BackgroundType, DrawType.Normal))
                {
                    case DrawType.Normal:
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, (this.LocalBounds.Center - texture.Bounds.Center).ToVector2(), texture.Bounds, Color.White);
                        break;
                    case DrawType.Stretch:
                        spriteBatch.Begin(samplerState: SamplerState.AnisotropicClamp);
                        spriteBatch.Draw(texture, this.LocalBounds, texture.Bounds, Color.White);
                        break;
                    case DrawType.Tile:
                        spriteBatch.Begin(samplerState: SamplerState.AnisotropicWrap);
                        spriteBatch.Draw(texture, this.LocalBounds, this.LocalBounds, Color.White);
                        break;
                }

                spriteBatch.End();
            }
        }
    }
}
