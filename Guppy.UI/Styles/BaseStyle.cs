using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.UI.Attributes;

namespace Guppy.UI.Styles
{
    public abstract class BaseStyle : IStyle
    {
        #region Static Fields
        protected internal static ElementState[] ElementStates = (ElementState[])Enum.GetValues(typeof(ElementState));
        protected internal static GlobalProperty[] GlobalProperties = (GlobalProperty[])Enum.GetValues(typeof(GlobalProperty));
        protected internal static StateProperty[] StateProperties = (StateProperty[])Enum.GetValues(typeof(StateProperty));
        protected internal static Dictionary<GlobalProperty, StylePropertyAttribute> GlobalPropertyAttributes = BaseStyle.GlobalProperties.ToDictionary(
            keySelector: p => p,
            elementSelector: p =>
            {
                return (StylePropertyAttribute)typeof(GlobalProperty).GetMember(p.ToString())[0]
                    .GetCustomAttributes(typeof(StylePropertyAttribute), false)[0];
            });
        protected internal static Dictionary<StateProperty, StylePropertyAttribute> StatePropertyAttributes = BaseStyle.StateProperties.ToDictionary(
            keySelector: p => p,
            elementSelector: p =>
            {
                return (StylePropertyAttribute)typeof(StateProperty).GetMember(p.ToString())[0]
                    .GetCustomAttributes(typeof(StylePropertyAttribute), false)[0];
            });
        #endregion

        #region Protected Fields
        protected internal Dictionary<GlobalProperty, Object> globalProperties;
        protected internal Dictionary<ElementState, Dictionary<StateProperty, Object>> stateProperties;
        #endregion

        protected BaseStyle()
        {
            this.globalProperties = new Dictionary<GlobalProperty, Object>(BaseStyle.GlobalProperties.Length);
            this.stateProperties = BaseStyle.ElementStates.ToDictionary(
                keySelector: s => s,
                elementSelector: s => new Dictionary<StateProperty, Object>(BaseStyle.StateProperties.Length));
        }

        #region Set Methods
        public void Set<TValue>(StateProperty property, TValue value)
        {
            this.Set<TValue>(ElementState.Normal, property, value);
        }
        public void Set<TValue>(ElementState state, StateProperty property, TValue value)
        {
            // First ensure that the input type is correct
            BaseStyle.StatePropertyAttributes[property].Assert(typeof(TValue));
            // Update the stored value
            this.stateProperties[state][property] = value;
        }
        public void Set<TValue>(GlobalProperty property, TValue value)
        {
            // First ensure that the input type is correct
            BaseStyle.GlobalPropertyAttributes[property].Assert(typeof(TValue));
            // Update the stored value
            this.globalProperties[property] = value;
        }
        #endregion

        #region Get Methods
        public abstract TValue Get<TValue>(GlobalProperty property, TValue fallback = default(TValue));
        public abstract TValue Get<TValue>(StateProperty property, TValue fallback = default(TValue));
        public abstract TValue Get<TValue>(ElementState state, StateProperty property, TValue fallback = default(TValue));
        #endregion
    }
}
