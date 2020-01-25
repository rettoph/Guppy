using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// The main element class. This defines basic
    /// sizing, debug rendering, child relationships,
    /// & more.
    /// </summary>
    public class Element : Entity
    {
        #region Private Fields
        private HashSet<Element> _children;
        #endregion

        #region Protected Attributes
        /// <summary>
        /// The scoped UI pointer instance.
        /// </summary>
        protected Pointer pointer { get; private set; }

        /// <summary>
        /// A collection of all entities (mainly useful for creating new entities)
        /// </summary>
        protected EntityCollection entities { get; private set; }

        /// <summary>
        /// If the current entity is dirty, it will be cleaned
        /// during its next update.
        /// </summary>
        protected Boolean dirty { get; set; }

        /// <summary>
        /// List of all children currently contained within the element
        /// </summary>
        protected IReadOnlyCollection<Element> children => _children;

        protected PrimitiveBatch primitiveBatch;
        protected SpriteBatch spritebatch;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The current bounds, as of the most recent cleaning.
        /// </summary>
        public ElementBounds Bounds { get; protected set; }

        /// <summary>
        /// The current element's parent (if any)
        /// </summary>
        public Element Parent { get; internal set; }

        /// <summary>
        /// The pointer flags (as of the most recent update)
        /// </summary>
        public PointerFlags PointerFlags { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _children = new HashSet<Element>();

            this.primitiveBatch = provider.GetRequiredService<PrimitiveBatch>();
            this.spritebatch = provider.GetRequiredService<SpriteBatch>();
            this.entities = provider.GetRequiredService<EntityCollection>();
            this.pointer = provider.GetRequiredService<Pointer>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.dirty = true;

            this.SetEnabled(false);
            this.SetVisible(false);

            this.Bounds = new ElementBounds(this);
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Bounds.OnChanged += this.HandleBoundsChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Bounds.OnChanged -= this.HandleBoundsChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.TryClean();

            if (this.Parent == null || this.Parent.PointerFlags.HasFlag(PointerFlags.Over))
            { // Only check the over status if the parent is currently hovered...
                this.PointerFlags = this.Bounds.Contains(this.pointer.Position) ? this.PointerFlags | PointerFlags.Over : this.PointerFlags & ~PointerFlags.Over;
            }
            else if(this.PointerFlags.HasFlag(PointerFlags.Over))
            { // Assume the over status is false...
                this.PointerFlags &= ~PointerFlags.Over;
            }

            
            this.children.TryUpdateAll(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.children.TryDrawAll(gameTime);

            this.primitiveBatch.DrawRectangle(this.Bounds, Color.Green);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Attempt to clean the current element.
        /// This will also call clean on all internal
        /// children (if any)
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

        /// <summary>
        /// This should be the real brains behind the public 
        /// TryClean method
        /// </summary>
        protected virtual void Clean()
        {
            this.Bounds.Clean();
            this.children.ForEach(c => c.dirty = true);
        }

        public void Add(Element child)
        {
            if (child is Stage)
                throw new Exception("Unable to add a stage into another Element. Stage must always be the root most element.");

            if(_children.Add(child))
            {
                child.Parent = this;
                child.dirty = true;
            }
        }
        public void Add<T>(String handle = default(String), Action<T> setup = null, Action<T> create = null)
            where T : Element
        {
            this.Add(this.entities.Create<T>(handle, setup, create));
        }
        public void Add<T>(Action<T> setup = null, Action<T> create = null)
            where T : Element
        {
            this.Add(this.entities.Create<T>(setup, create));
        }

        public void Remove(Element child)
        {
            _children.Remove(child);
        }

        protected internal virtual Rectangle GetParentBounds()
        {
            return this.Parent.Bounds;
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsChanged(object sender, EventArgs e)
        {
            this.dirty = true;
        }
        #endregion
    }
}
