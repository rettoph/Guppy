using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Elements
{
    public class ScrollItems : Container
    {
        private ScrollContainer _container;

        public ScrollItems(ScrollContainer container) : base(0, 0, new UnitValue[] { 1f, -15 }, 1f)
        {
            _container = container;

            this.StateBlacklist = ElementState.Active | ElementState.Pressed | ElementState.Hovered;

            _container.Inner.OnBoundsChanged += this.HandleParentBoundsChanged;
        }

        public override void Clean()
        {
            base.Clean();

            this.align();
        }

        public override void CleanBounds()
        {
            base.CleanBounds();

            var inline = this.Style.Get<Boolean>(GlobalProperty.InlineContent, true);

            if (inline)
            { // If the current element is marked for inline content... re-order inner element automatically
                var y = 0;
                foreach (Element child in this.children)
                {
                    child.Outer.Y.SetValue(y);
                    y += child.Outer.Height.Value;
                }
            }
        }

        public override void Add(Element child)
        {
            base.Add(child);

            // Make the childs bounds parent the scroll container
            child.Outer.Height.SetParent(_container.Inner.Height);

            var inline = this.Style.Get<Boolean>(GlobalProperty.InlineContent, true);
            
            this.DirtyPosition = true;
        }

        public override void Remove(Element child)
        {
            base.Add(child);

            this.DirtyPosition = true;
        }

        protected internal void align()
        {
            if (this.children.Count > 0)
            {
                this.Outer.Height.SetValue(this.children.Max(e => e.Outer.RelativeBounds.Bottom));
                this.Outer.Y.SetValue((Int32)((this.Outer.LocalBounds.Height - _container.Inner.LocalBounds.Height) * -_container.Scroll));
            }
        }

        private void HandleParentBoundsChanged(object sender, Rectangle e)
        {
            this.DirtyBounds = true;
        }
    }
}
