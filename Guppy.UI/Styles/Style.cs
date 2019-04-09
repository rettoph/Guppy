using Guppy.UI.Attributes;
using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Guppy.UI.Utilities.Units.UnitValues;

namespace Guppy.UI.Styles
{
    public class Style
    {
        #region Static Fields
        protected internal static StyleProperty[] StyleProperties = (StyleProperty[])Enum.GetValues(typeof(StyleProperty));
        protected internal static GlobalStyleProperty[] GlobalStyleProperties = (GlobalStyleProperty[])Enum.GetValues(typeof(GlobalStyleProperty));
        protected internal static ElementState[] ElementStates = (ElementState[])Enum.GetValues(typeof(ElementState));
        protected internal static Dictionary<StyleProperty, StylePropertyAttribute> StylePropertyAttributes = Style.StyleProperties.ToDictionary(
            keySelector: p => p,
            elementSelector: p =>
            {
                return (StylePropertyAttribute)typeof(StyleProperty).GetMember(p.ToString())[0]
                    .GetCustomAttributes(typeof(StylePropertyAttribute), false)[0];
            });
        protected internal static Dictionary<GlobalStyleProperty, StylePropertyAttribute> GlobalStylePropertyAttributes = Style.GlobalStyleProperties.ToDictionary(
            keySelector: p => p,
            elementSelector: p =>
            {
                return (StylePropertyAttribute)typeof(GlobalStyleProperty).GetMember(p.ToString())[0]
                    .GetCustomAttributes(typeof(StylePropertyAttribute), false)[0];
            });
        #endregion

        #region Protected Fields
        protected Dictionary<GlobalStyleProperty, Object> globalStyleProperties;
        protected Dictionary<ElementState, Dictionary<StyleProperty, Object>> styleProperties;
        protected Style root;
        #endregion

        #region Constructors
        public Style(Style root = null)
        {
            this.root = root;
            this.globalStyleProperties = new Dictionary<GlobalStyleProperty, Object>();
            this.styleProperties = new Dictionary<ElementState, Dictionary<StyleProperty, Object>>(Style.ElementStates.Length);
            foreach (ElementState state in Style.ElementStates)
                this.styleProperties.Add(state, new Dictionary<StyleProperty, Object>(Style.StyleProperties.Length));

            if (this.root == null)
            { // If no root style is defined, we should set some defualts
                this.Set<Color>(ElementState.Normal, StyleProperty.OuterDebugBoundaryColor, Color.Red);
                this.Set<Color>(ElementState.Hovered, StyleProperty.OuterDebugBoundaryColor, Color.Blue);
                this.Set<Color>(ElementState.Active, StyleProperty.OuterDebugBoundaryColor, Color.Orange);
                this.Set<Color>(ElementState.Pressed, StyleProperty.OuterDebugBoundaryColor, Color.Green);
                this.Set<Color>(ElementState.Normal, StyleProperty.InnerDebugBoudaryColor, Color.Gray);
                this.Set<Color>(ElementState.Active, StyleProperty.InnerDebugBoudaryColor, Color.DarkGray);

                this.Set<UnitValue>(GlobalStyleProperty.PaddingTop    , 5);
                this.Set<UnitValue>(GlobalStyleProperty.PaddingRight  , 5);
                this.Set<UnitValue>(GlobalStyleProperty.PaddingBottom , 5);
                this.Set<UnitValue>(GlobalStyleProperty.PaddingLeft   , 5);
            }
        }
        #endregion

        #region State Properties
        public virtual TProperty Get<TProperty>(ElementState state, StyleProperty property, TProperty fallback = default(TProperty))
        {
            if (this.styleProperties[state].ContainsKey(property))
                return (TProperty)this.styleProperties[state][property];
            else if (Style.StylePropertyAttributes[property].Inherit && this.root != null)
                return this.root.Get<TProperty>(state, property, fallback);
            else if (this.styleProperties[ElementState.Normal].ContainsKey(property))
                return this.Get<TProperty>(ElementState.Normal, property, fallback);
            else
                return fallback;
        }

        public void Set<TProperty>(ElementState state, StyleProperty property, TProperty value)
        {
            // Assert that a valid type was recieved
            Style.StylePropertyAttributes[property].Assert(value);

            this.styleProperties[state][property] = value;
        }
        #endregion

        #region Global Properties
        public virtual TProperty Get<TProperty>(GlobalStyleProperty property)
        {
            if (this.globalStyleProperties.ContainsKey(property))
                return (TProperty)this.globalStyleProperties[property];
            else if (Style.GlobalStylePropertyAttributes[property].Inherit && this.root != null)
                return this.root.Get<TProperty>(property);
            else
                throw new Exception($"Unable to load global style property! GlobalStyleProperty => {property}");
        }

        public void Set<TProperty>(GlobalStyleProperty property, TProperty value)
        {
            // Assert that a valid type was recieved
            Style.GlobalStylePropertyAttributes[property].Assert(value);

            this.globalStyleProperties[property] = value;
        }
        #endregion
    }
}
