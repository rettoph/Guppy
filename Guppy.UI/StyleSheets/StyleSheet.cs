using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Guppy.UI.StyleSheets
{
    public class StyleSheet
    {
        private Dictionary<ElementState, Dictionary<StyleProperty, Object>> _styles;

        public StyleSheet()
        {
            _styles = ((ElementState[])Enum.GetValues(typeof(ElementState)))
                .ToDictionary(
                    keySelector: es => es,
                    elementSelector: es => new Dictionary<StyleProperty, Object>(Enum.GetValues(typeof(StyleProperty)).Length));

            // Set the default debug overlay colors
            this.SetProperty(ElementState.Normal, StyleProperty.DebugColor, Color.Red);
            this.SetProperty(ElementState.Hovered, StyleProperty.DebugColor, Color.Blue);
            this.SetProperty(ElementState.Active, StyleProperty.DebugColor, Color.Green);
        }

        public virtual TProperty GetProperty<TProperty>(ElementState state, StyleProperty property)
        {
            if (_styles[state].ContainsKey(property))
                return (TProperty)_styles[state][property];
            else if (_styles[ElementState.Normal].ContainsKey(property))
                return (TProperty)_styles[ElementState.Normal][property];
            else
                return default(TProperty);
        }

        public virtual void SetProperty(ElementState state, StyleProperty property, Object value)
        {
            _styles[state][property] = value;
        }
    }
}
