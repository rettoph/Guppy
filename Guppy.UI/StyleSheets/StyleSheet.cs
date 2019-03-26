using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.StyleSheets
{
    public class StyleSheet : BaseStyleSheet
    {
        public StyleSheet()
        {
            // Set the default debug overlay colors
            this.SetProperty(ElementState.Normal, StyleProperty.DebugColor, Color.Red);
            this.SetProperty(ElementState.Hovered, StyleProperty.DebugColor, Color.Blue);
            this.SetProperty(ElementState.Active, StyleProperty.DebugColor, Color.Green);
        }
    }
}
