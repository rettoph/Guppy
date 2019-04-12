using Guppy.UI.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum StateProperty
    {
        /*
         *  Background Properties
         */
        [StyleProperty(typeof(Texture2D), false)]
        Background,
        [StyleProperty(typeof(DrawType), false)]
        BackgroundType,

        /*
         * Debug Properties
         */
        [StyleProperty(typeof(Color), true)]
        OuterDebugColor,
        [StyleProperty(typeof(Color), true)]
        InnerDebugColor,
    }
}
