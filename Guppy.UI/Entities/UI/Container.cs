using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Entities.UI.Interfaces;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    public class Container<TElement> : Element, IContainer<TElement>
        where TElement : Element
    {
        #region Private Fields
        private HashSet<TElement> _children;
        private EntityCollection _entities;
        #endregion

        #region Protected Fields
        public IReadOnlyCollection<TElement> Children => _children;
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
        }

        public override void Dispose()
        {
            base.Dispose();

            while (_children.Any())
                this.Remove(_children.First());
        }
        #endregion

        #region Frame Methods 
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _children.TryUpdateAll(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _children.TryDrawAll(gameTime);
        }
        #endregion

        #region Methods
        protected override void Clean()
        {
            base.Clean();

            // Mark all internal children dirty too
            _children.ForEach(c => c.dirty = true);
        }
        #endregion

        #region Event Handlers
        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            this.dirty = true;
        }
        #endregion

        #region IContainer Implementation
        public TElement Add(TElement child)
        {
            _children.Add(child);
            child.container = this;

            return child;
        }

        public T Add<T>(string handle, Action<T> setup = null, Action<T> create = null) where T : TElement
        {
            return this.Add(_entities.Create<T>(handle, setup, create)) as T;
        }

        public T Add<T>(Action<T> setup = null, Action<T> create = null) where T : TElement
        {
            return this.Add(_entities.Create<T>(setup, create)) as T;
        }

        public void Remove(TElement child)
        {
            _children.Remove(child);
        }
        #endregion
    }
}
