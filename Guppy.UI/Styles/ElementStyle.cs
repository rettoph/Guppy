using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Elements;
using Guppy.UI.Enums;

namespace Guppy.UI.Styles
{
    /// <summary>
    /// Implementation of the style interface meant to be contained
    /// directly within an element. This will manage inheritance
    /// on an as needed basis.
    /// </summary>
    public class ElementStyle : BaseStyle
    {
        public readonly Element Parent;
        public Style Root { get; set; }

        public ElementStyle(Element parent, Style root)
        {
            this.Parent = parent;
            this.Root = root;
        }

        public override TValue Get<TValue>(GlobalProperty property, TValue fallback = default(TValue))
        {
            TValue rootVal;

            if (this.globalProperties.ContainsKey(property))
                return (TValue)this.globalProperties[property];
            else if(this.Root != null && (rootVal = this.Root.Get<TValue>(property, fallback)) != null && !rootVal.Equals(fallback))
                return rootVal;
            else if(BaseStyle.GlobalPropertyAttributes[property].Inherit && this.Parent.Parent != null)
                return this.Parent.Parent.Style.Get<TValue>(property, fallback);

            return fallback;
        }

        public override TValue Get<TValue>(StateProperty property, TValue fallback = default(TValue))
        {
            return this.Get<TValue>(this.Parent.State, property, fallback);
        }

        public override TValue Get<TValue>(ElementState state, StateProperty property, TValue fallback = default(TValue))
        {
            TValue rootVal;

            if (this.stateProperties[state].ContainsKey(property))
                return (TValue)this.stateProperties[state][property];
            else if (this.stateProperties[ElementState.Normal].ContainsKey(property))
                return (TValue)this.stateProperties[ElementState.Normal][property];
            else if (this.Root != null && (rootVal = this.Root.Get<TValue>(state, property, fallback)) != null && !rootVal.Equals(fallback))
                return rootVal;
            else if (BaseStyle.StatePropertyAttributes[property].Inherit && this.Parent.Parent != null)
                return this.Parent.Parent.Style.Get<TValue>(state, property, fallback);

            return fallback;
        }
    }
}
