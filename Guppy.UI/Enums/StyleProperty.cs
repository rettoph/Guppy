using Guppy.UI.Attributes;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum StyleProperty
    {
        /*
         * Background Properties
         */
        [StyleProperty(typeof(Texture2D), true)]
        BackgroundImage,
        [StyleProperty(typeof(DrawType), true)]
        BackgroundType,

        /*
         * Text Properties
         */
        [StyleProperty(typeof(SpriteFont), true)]
        Font,
        [StyleProperty(typeof(Alignment), true)]
        TextAlignment,

        /*
         * Debug Properties
         */
        [StyleProperty(typeof(Color), true)]
        OuterDebugBoundaryColor,
        [StyleProperty(typeof(Color), true)]
        InnerDebugBoudaryColor
    }
}
