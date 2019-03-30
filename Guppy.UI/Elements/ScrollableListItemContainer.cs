using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.Utilities.Units;

namespace Guppy.UI.Elements
{
    public class ScrollableListItemContainer : SimpleContainer
    {
        private ScrollableList _parent;
        private Int32 _itemOffset;
        private Unit _itemSpacing;

        protected internal ScrollableListItemContainer(ScrollableList parent) : base(0, 0, 1f, 1f)
        {
            _parent = parent;

            this.Parent = parent;
        }

        protected internal override void UpdateCache()
        {
            // Ensure that the container width is compensating for the width of the scrolbar
            this.Width = new Unit[] { 1f, -_parent.ScrollBarContainer.Width };

            // Reposition all the children and update the container height
            if (this.children.Count == 0)
                this.Height = 0;
            else
            {
                _itemSpacing = _parent.StyleSheet.GetProperty<Unit>(ElementState.Normal, StyleProperty.ListItemSpacing);
                _itemOffset = _itemSpacing;
                

                foreach (Element child in this.children)
                {
                    child.Y = _itemOffset;
                    _itemOffset += child.Height + _itemSpacing;
                }

                this.Height = _itemOffset;
            }

            base.UpdateCache();
        }

        #region Adders & Removers
        public override TELement Add<TELement>(TELement child)
        {
            this.DirtyBounds = true;

            return base.Add(child);
        }

        public override Element Remove(Element child)
        {
            this.DirtyBounds = true;

            return base.Remove(child);
        }
        #endregion
    }
}
