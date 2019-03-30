using Guppy.UI.Attributes;
using Guppy.UI.Utilities.Units;
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
         * Font & Text Properties
         */
        [StylePropertyType(typeof(Color))]
        FontColor,
        [StylePropertyType(typeof(SpriteFont))]
        Font,
        [StylePropertyType(typeof(Alignment))]
        TextAlignment,

        /* 
         * Padding Properties
         */
        [StylePropertyType(typeof(Unit))]
        PaddingTop,
        [StylePropertyType(typeof(Unit))]
        PaddingRight,
        [StylePropertyType(typeof(Unit))]
        PaddingBottom,
        [StylePropertyType(typeof(Unit))]
        PaddingLeft,

        /*
         * Background Properties
         */
        [StylePropertyType(typeof(Texture2D))]
        BackgroundImage,

        /*
         * Scrollable Properties
         */

        [StylePropertyType(typeof(Unit))]
        ListItemSpacing,
        [StylePropertyType(typeof(Color))]
        ScrollBarColor,
        [StylePropertyType(typeof(Color))]
        ScrollBarHandleColor,

        /*
         * Debug Properties
         */
        [StylePropertyType(typeof(Color))]
        DebugWireframeColor,
        [StylePropertyType(typeof(Color))]
        DebugPaddingColor,
    }
}
