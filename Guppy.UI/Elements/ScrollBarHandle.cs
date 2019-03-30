using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    public class ScrollBarHandle : SimpleElement
    {
        private ScrollBar _parent;

        protected internal ScrollBarHandle(ScrollBar parent, StyleSheet rootStyleSheet = null) : base(0, 0, 1f, 1f, rootStyleSheet)
        {
            _parent = parent;
            this.Parent = parent;
        }

        protected override void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            this.graphicsDevice.Clear(this.StyleSheet.GetProperty<Color>(state, StyleProperty.ScrollBarHandleColor));

            base.generateTexture(state, ref target);
        }
    }
}
