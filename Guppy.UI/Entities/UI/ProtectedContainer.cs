using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
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
        }

        public override void Dispose()
        {
            base.Dispose();

            while (_children.Any())
                this.remove(_children.First());
        }
        #endregion

        #region Clean Methods
        protected override void Clean()
        {
            base.Clean();

            // Mark all internal children dirty too
            _children.ForEach(c => c.dirty = true);
        }
        #endregion

        #region Container Methods
        protected TElement add(TElement child)
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

        protected void remove(TElement child)
        {
            _children.Remove(child);
            this.dirty = true;
        }
        #endregion
    }
}
