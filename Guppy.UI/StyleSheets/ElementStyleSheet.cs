using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Elements;
using Guppy.UI.Enums;

namespace Guppy.UI.StyleSheets
{
    /// <summary>
    /// Represents element specific styles. These styles can be overwritten on
    /// an element specific level.
    /// </summary>
    public class ElementStyleSheet : BaseStyleSheet
    {
        private Element _parent;

        public StyleSheet Root { get; set; }

        public ElementStyleSheet(StyleSheet root, Element parent)
        {
            this.Root = root;
            _parent = parent;
        }

        public override TProperty GetProperty<TProperty>(ElementState state, StyleProperty property)
        {
            var internalStyle =  base.GetProperty<TProperty>(state, property);

            if (internalStyle == null || internalStyle.Equals(default(TProperty))) {
                if (this.Root != null)
                { // If the element doesnt have a value defined, fallback on the root value
                    var rootStyle = this.Root.GetProperty<TProperty>(state, property);

                    if (rootStyle == null || rootStyle.Equals(default(TProperty)))
                    {
                        return _parent.Stage.StyleSheet.GetProperty<TProperty>(state, property);
                    }

                    return rootStyle;
                }
                else
                {
                    return _parent.Stage.StyleSheet.GetProperty<TProperty>(state, property);
                }
            }
            

            return internalStyle;
        }
    }
}
