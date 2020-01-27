using Guppy.Extensions.Collection;
using Guppy.UI.Entities.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Special container meant to contain text elements.
    /// Each text element will automatically be resized and positioned
    /// next to its sibling element.
    /// </summary>
    public sealed class InlineTextContainer : StyleElement, IContainer<TextElement>
    {
        #region Clean Methods
        protected override void Clean()
        {
            // We want to update & clean internal elements first, as the bounds
            // are updated here.
            this.CleanTextElements();

            // Call base.Clean, where the bounds are cleaned...
            base.Clean();
        }

        private void CleanTextElements()
        {
            Int32 offset = 0;
            Vector2 size = Vector2.Zero;

            this.children.ForEach(c => {
                size = (c as TextElement).Font.MeasureString((c as TextElement).Text);
                c.Bounds.Top = 0;
                c.Bounds.Left = offset;
                c.Bounds.Width = (Int32)size.X;
                offset += (Int32)size.X;
            });

            // Update the container bounds
            this.Bounds.Width = offset;
        }
        #endregion

        #region IContainer Implementation
        /// <inheritdoc />
        public TextElement Add(TextElement child)
        {
            return this.add(child) as TextElement;
        }

        /// <inheritdoc />
        public T Add<T>(String handle, Action<T> setup = null, Action<T> create = null) 
            where T : TextElement
        {
            return this.add<T>(handle, setup, create);
        }

        /// <inheritdoc />
        public T Add<T>(Action<T> setup = null, Action<T> create = null) 
            where T : TextElement
        {
            return this.add<T>(setup, create);
        }

        /// <inheritdoc />
        public void Remove(TextElement child)
        {
            this.remove(child);
        }
        #endregion
    }
}
