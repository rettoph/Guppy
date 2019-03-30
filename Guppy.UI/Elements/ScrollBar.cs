using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Elements
{
    public class ScrollBar : SimpleElement
    {
        private ScrollableList _parent;
        public ScrollBarHandle Handle { get; set; }
        public Single Value { get; private set; }

        public ScrollBar(ScrollableList parent, StyleSheet rootStyleSheet = null) : base(new Unit[] { 1f, -15 }, 0, 15, 1f, rootStyleSheet)
        {
            _parent = parent;
            this.Parent = parent;

            this.Handle = new ScrollBarHandle(this, rootStyleSheet);
            this.Value = 0f;
        }

        #region Frame Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            this.Handle.Draw(gameTime, spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Handle.Update(gameTime);
        }
        #endregion

        public void Scroll(Single amount)
        {
            this.Value += amount;

            if (this.Value > 1)
                this.Value = 1;
            else if (this.Value < 0)
                this.Value = 0;

            _parent.DirtyBounds = true;
        }
        public void ScrollTo(Single amount)
        {
            this.Value = amount;

            if (this.Value > 1)
                this.Value = 1;
            else if (this.Value < 0)
                this.Value = 0;

            _parent.DirtyBounds = true;
        }

        protected internal override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            base.AddDebugVertices(ref vertices);

            this.Handle.AddDebugVertices(ref vertices);
        }
        protected internal override void UpdateCache()
        {
            base.UpdateCache();

            this.Handle.UpdateCache();
        }

        protected internal override void UpdateBounds()
        {
            base.UpdateBounds();

            if (_parent.Items.Height > 0 && _parent.Height < _parent.Items.Height)
            {
                this.Handle.Height = ((Single)_parent.Height / (Single)_parent.Items.Height);
                this.Handle.UpdateBounds(this.Bounds);
                this.Handle.Y = (Int32)((this.Height - this.Handle.Height) * this.Value);
            }
            else
            {
                this.Handle.Height = 1f;
                this.Handle.Y = 0;
                this.Value = 0;
            }

            this.Handle.UpdateBounds();
        }
    }
}
