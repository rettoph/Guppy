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
    public class Caret : SimpleElement
    {
        protected internal Caret(TextInput parent, Unit x, Unit y, Unit width, Unit height, StyleSheet rootStyleSheet = null) : base(x, y, width, height, rootStyleSheet)
        {
            this.Parent = parent;
        }

        protected override void generateTexture(ElementState state, ref RenderTarget2D target)
        {
            base.generateTexture(state, ref target);

            this.graphicsDevice.Clear(this.StyleSheet.GetProperty<Color>(state, StyleProperty.FontColor));
        }
    }
}
