using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Styles
{
    public class Style : BaseStyle
    {
        #region Get Methods
        public override TValue Get<TValue>(GlobalProperty property, TValue fallback = default(TValue))
        {
            if (this.globalProperties.ContainsKey(property))
                return (TValue)this.globalProperties[property];

            return fallback;
        }

        public override TValue Get<TValue>(StateProperty property, TValue fallback = default(TValue))
        {
            return this.Get<TValue>(ElementState.Normal, property);
        }

        public override TValue Get<TValue>(ElementState state, StateProperty property, TValue fallback = default(TValue))
        {
            if (this.stateProperties[state].ContainsKey(property))
                return (TValue)stateProperties[state][property];
            if (this.stateProperties[ElementState.Normal].ContainsKey(property))
                return (TValue)this.stateProperties[ElementState.Normal][property];

            return fallback;
        }
        #endregion
    }
}
