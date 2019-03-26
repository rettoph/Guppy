using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;

namespace Guppy.UI.StyleSheets
{
    /// <summary>
    /// Represents element specific styles. These styles can be overwritten on
    /// an element specific level.
    /// </summary>
    public class ElementStyleSheet : StyleSheet
    {
        public StyleSheet Root { get; set; }

        public ElementStyleSheet(StyleSheet root = null)
        {
            this.Root = root;
        }

        public override TProperty GetProperty<TProperty>(ElementState state, StyleProperty property)
        {
            var internalStyle =  base.GetProperty<TProperty>(state, property);

            if(internalStyle.Equals(default(TProperty)) && this.Root != null)
            { // If the element doesnt have a value defined, fallback on the root value
                return this.Root.GetProperty<TProperty>(state, property);
            }

            return internalStyle;
        }
    }
}
