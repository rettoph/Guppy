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

        private Boolean _dirtyAlignment;

        public ScrollItems(UnitRectangle outerBounds, ScrollContainer parent, Stage stage) : base(outerBounds, parent, stage)
        {
            this.MaxItems = Int32.MaxValue;

            _container = parent;
            _dirtyAlignment = false;

            this.StateBlacklist = ElementState.Active | ElementState.Pressed | ElementState.Hovered;
            this.SetPadding(0, 0, 0, 0);

            _container.Inner.OnBoundsChanged += this.HandleParentBoundsChanged;
        }

        public override void Update(MouseState mouse)
        {
            base.Update(mouse);

            if(_dirtyAlignment)
            {
                this.align();
                _dirtyAlignment = false;
            }
        }
        public override void Clean()
        {
            this.resize();
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
            child.Outer.Update(); // Ensure the child bounds get updated at least once

            // Remove any elements as needed
            if (this.children.Count > this.MaxItems)
                this.remove(this.children.ElementAt(0));

            // Re-align the inner children
            _dirtyAlignment = true;

            // Add event handler for when children are independantly re-aligned
            child.Outer.Height.OnUpdated += this.HandleItemHeightUpdated;

            return child;
        }

        public override void Remove(Element child)
        {
            base.Remove(child);

            _dirtyAlignment = true;
        }

        protected internal void align()
        {
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

            this.resize();
        }

        protected internal void resize()
        {
            if (this.children.Count > 0)
            {
                this.Outer.Height.SetValue(this.children.Max(e => e.Outer.RelativeBounds.Bottom));
                this.Outer.Y.SetValue((Int32)((this.Outer.LocalBounds.Height - _container.Inner.LocalBounds.Height) * -_container.Scroll));
                Console.WriteLine(this.Outer.Y.Value);
            }
        }

        private void HandleParentBoundsChanged(object sender, Rectangle e)
        {
            _dirtyAlignment = true;
        }

        private void HandleItemHeightUpdated(object sender, Unit e)
        {
            _dirtyAlignment = true;
        }

        public override void Dispose()
        {
            foreach (Element child in this.children)
                child.Outer.Height.OnUpdated -= this.HandleItemHeightUpdated;

            base.Dispose();

            _container.Inner.OnBoundsChanged -= this.HandleParentBoundsChanged;

            _container = null;
        }
    }
}
