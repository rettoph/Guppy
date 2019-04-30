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
using Guppy.UI.Entities;
using Guppy.UI.Utilities;

namespace Guppy.UI.Elements
{
    public class ScrollItems : Container
    {
        public Int32 MaxItems { get; set; }

        private ScrollContainer _container;

        public ScrollItems(UnitRectangle outerBounds, ScrollContainer parent, Stage stage) : base(outerBounds, parent, stage)
        {
            this.MaxItems = Int32.MaxValue;

            _container = parent;

            this.StateBlacklist = ElementState.Active | ElementState.Pressed | ElementState.Hovered;
            this.SetPadding(0, 0, 0, 0);

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

        /// <summary>
        /// Create a new element and automatically add it as a child
        /// of the current element.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override TElement CreateElement<TElement>(UnitValue x, UnitValue y, UnitValue width, UnitValue height, params Object[] args)
        {
            // Update the y value automatically if inline content is true
            if (this.children.Count > 0 && this.Style.Get<Boolean>(GlobalProperty.InlineContent, true))
                y = this.children.Last().Outer.RelativeBounds.Bottom;

            var child = this.createElement<TElement>(x, y, width, height, args);

            // Make the childs bounds parent the scroll container
            child.Outer.Height.SetParent(_container.Inner.Height);

            // Remove any elements as needed
            if (this.children.Count > this.MaxItems)
                this.remove(this.children.ElementAt(0));

            this.DirtyPosition = true;

            return child;
        }

        public override void Remove(Element child)
        {
            base.Remove(child);

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
