using Guppy.Extensions.Collection;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Defines the minimum templating required for a UI element.
    /// </summary>
    public abstract class Element : Entity
    {
        #region Private Fields
        /// <summary>
        /// A list of all children within the current element
        /// </summary>
        private HashSet<Element> _children;

        /// <summary>
        /// Indicates that the current element is the top most element
        /// the pointer is currently hovering over.
        /// </summary>
        private Boolean _top;
        #endregion

        #region Protected Fields
        protected abstract SpriteBatch spriteBatch { get; }
        protected abstract PrimitiveBatch primitiveBatch { get; }
        protected abstract Pointer pointer { get; }
        protected Boolean dirty { get; set; }
        #endregion

        #region Public Attributes
        public ElementBounds Bounds { get; private set; }
        public Boolean Hovered { get; private set; }
        #endregion

        #region Lifecycle Methods 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Bounds = new ElementBounds(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.dirty = true;

            _children = new HashSet<Element>();
        }

        public override void Dispose()
        {
            base.Dispose();

            _children.ForEach(c => c.Dispose());
            _children.Clear();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _children.TryDrawAll(gameTime);

            this.primitiveBatch.DrawRectangle(this.Bounds.Pixel, Color.Red);
        }

        protected override void PreUpdate(GameTime gameTIme)
        {
            base.PreUpdate(gameTIme);

            this.TryClean();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _top = true;
            _children.ForEach(c =>
            {
                c.TryUpdate(gameTime);
                _top &= !(c.Hovered || !c._top);
            });
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Return the current element's container's bounds.
        /// 
        /// This should be in pixels
        /// </summary>
        /// <returns></returns>
        public abstract Rectangle GetContainerBounds();
        #endregion

        #region Clean Methods
        /// <summary>
        /// Attempt to clean the current element.
        /// This will also call mark all internal
        /// children as dirty.
        /// </summary>
        /// <param name="force"></param>
        public void TryClean(Boolean force = false)
        {
            if(this.dirty || force)
            {
                this.Clean();
                this.dirty = false;
            }
        }

        protected virtual void Clean()
        {
            this.Bounds.Clean();
            _children.ForEach(c => c.dirty = true);
        }
        #endregion
    }
}
