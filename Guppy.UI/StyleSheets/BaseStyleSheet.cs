using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Guppy.UI.Attributes;

namespace Guppy.UI.StyleSheets
{
    public abstract class BaseStyleSheet
    {
        private static Type PropertyType = typeof(StyleProperty);
        private static Type PropertyTypeAttribute = typeof(StylePropertyTypeAttribute);

        private Dictionary<ElementState, Dictionary<StyleProperty, Object>> _styles;

        public BaseStyleSheet()
        {
            _styles = ((ElementState[])Enum.GetValues(typeof(ElementState)))
                .ToDictionary(
                    keySelector: es => es,
                    elementSelector: es => new Dictionary<StyleProperty, Object>(Enum.GetValues(typeof(StyleProperty)).Length));
        }

        /// <summary>
        /// Get a specified property value
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="state"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual TProperty GetProperty<TProperty>(ElementState state, StyleProperty property)
        {
            if (_styles[state].ContainsKey(property))
                return (TProperty)_styles[state][property];
            else if (_styles[ElementState.Normal].ContainsKey(property))
                return (TProperty)_styles[ElementState.Normal][property];
            else
                return default(TProperty);
        }

        /// <summary>
        /// Set a property for the normal state
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public virtual void SetProperty<TValue>(StyleProperty property, TValue value)
        {
            this.SetProperty(ElementState.Normal, property, value);
        }


        /// <summary>
        /// Set a property for a specified state
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="state"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public virtual void SetProperty<TValue>(ElementState state, StyleProperty property, TValue value)
        {
            var typeAttribute = (StylePropertyTypeAttribute)BaseStyleSheet.PropertyType.GetMember(property.ToString())[0]
                .GetCustomAttributes(BaseStyleSheet.PropertyTypeAttribute, false)[0];

            // Assert that a valid type was recieved
            typeAttribute.Assert(value);

            _styles[state][property] = value;
        }
    }
}
