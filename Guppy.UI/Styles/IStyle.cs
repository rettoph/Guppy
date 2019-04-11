using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Styles
{
    public interface IStyle
    {
        #region Set Methods
        void Set<TValue>(GlobalProperty property, TValue value);
        void Set<TValue>(StateProperty property, TValue value);
        void Set<TValue>(ElementState state, StateProperty property, TValue value);
        #endregion

        #region Get Methods
        TValue Get<TValue>(GlobalProperty property, TValue fallback = default(TValue));
        TValue Get<TValue>(StateProperty property, TValue fallback = default(TValue));
        TValue Get<TValue>(ElementState state, StateProperty property, TValue fallback = default(TValue));
        #endregion
    }
}
