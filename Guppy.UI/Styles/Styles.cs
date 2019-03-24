using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Styles
{
    public class Styles
    {
        private Dictionary<ElementState, Dictionary<StyleProperty, Object>> _styles;

        public Styles()
        {
            _styles = ((ElementState[])Enum.GetValues(typeof(ElementState)))
                .ToDictionary(
                    keySelector: es => es,
                    elementSelector: es => new Dictionary<StyleProperty, Object>(Enum.GetValues(typeof(StyleProperty)).Length));
        }

        public TProperty GetProperty<TProperty>(ElementState state, StyleProperty property)
        {
            if (_styles[state].ContainsKey(property))

                return (TProperty)_styles[state][property];
            else if (_styles[ElementState.Normal].ContainsKey(property))
                return (TProperty)_styles[state][property];
            else
                return default(TProperty);
        }
    }
}
