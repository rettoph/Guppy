using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.DependencyInjection;

namespace Guppy.Collections
{
    public sealed class LayerEntityCollection : OrderableCollection<Entity>
    {
        #region Private Fields
        private EntityCollection _entities;
        #endregion

        #region Internal Fields
        internal Layer layer;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _entities = provider.GetService<EntityCollection>();
        }
        #endregion

        #region Collection Methods
        protected override void Add(Entity item)
        {
            base.Add(item);

            // Remove from the old layer...
            item.Layer?.Entities.TryRemove(item);
            // Update the internal layer value...
            item.Layer = this.layer;
        }
        #endregion

        #region Factory Methods
        protected override Entity Create(ServiceProvider provider, Type serviceType, Action<Entity, ServiceProvider, ServiceConfiguration> setup = null)
        {
            var entity = base.Create(provider, serviceType, setup);
            entity.LayerGroup = this.layer.Group.GetValue();

            // Automatically add the new entity into the global entity collection.
            _entities.TryAdd(entity);

            return entity;
        }

        protected override Entity Create(ServiceProvider provider, uint id, Action<Entity, ServiceProvider, ServiceConfiguration> setup = null)
        {
            var entity = base.Create(provider, id, setup);
            entity.LayerGroup = this.layer.Group.GetValue();

            // Automatically add the new entity into the global entity collection.
            _entities.TryAdd(entity);

            return entity;
        }
        #endregion
    }
}
