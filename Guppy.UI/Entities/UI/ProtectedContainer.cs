using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Represents an element that is a secret container and does not allow
    /// elements to publicly be added.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class ProtectedContainer<TElement> : FancyElement
        where TElement : Element
    {
        #region Private Fields
        private HashSet<TElement> _children;
        private EntityCollection _entities;
        #endregion

        #region Protected Fields
        protected IReadOnlyCollection<TElement> children => _children;
        #endregion

        #region Lifecycle Method 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _entities = provider.GetRequiredService<EntityCollection>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            _children = new HashSet<TElement>();

            this.Bounds.Set(0, 0, Unit.Get(1f, -1), Unit.Get(1f, -1));

            this.OnBoundsChanged += this.HandleBoundsChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            while (_children.Any())
                this.remove(_children.First());

            this.OnBoundsChanged -= this.HandleBoundsChanged;
        }
        #endregion

        #region Frame Methods 
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.children.TryUpdateAll(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.children.TryDrawAll(gameTime);
        }
        #endregion

        #region Clean Methods

        #endregion

        #region Container Methods
        protected virtual TElement add(TElement child)
        {
            _children.Add(child);
            child.container = this;
            this.dirty = true;

            return child;
        }

        protected T add<T>(string handle, Action<T> setup = null, Action<T> create = null) where T : TElement
        {
            return this.add(_entities.Create<T>(handle, setup, create)) as T;
        }

        protected T add<T>(Action<T> setup = null, Action<T> create = null) where T : TElement
        {
            return this.add(_entities.Create<T>(setup, create)) as T;
        }

        protected virtual void remove(TElement child)
        {
            _children.Remove(child);
            this.dirty = true;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// When the container bounds are updated we need to clean 
        /// the children.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleBoundsChanged(object sender, Rectangle e)
        {
            _children.ForEach(c => c.TryClean(true));
        }
        #endregion
    }
}
