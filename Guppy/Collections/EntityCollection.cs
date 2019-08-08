using Guppy.Extensions.DependencyInjection;
using Guppy.Utilities.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    public class EntityCollection : FrameableCollection<Entity>
    {
        #region Private Fields
        private IServiceProvider _provider;
        private EntityLoader _entityLoader;
        private LayerCollection _layers;
        #endregion

        #region Constructors
        public EntityCollection(EntityLoader entityLoader, LayerCollection layers, IServiceProvider provider) : base(provider)
        {
            _entityLoader = entityLoader;
            _provider = provider;
            _layers = layers;
        }
        #endregion


        #region Collection Methods
        public override bool Add(Entity item)
        {
            if (base.Add(item))
            { // When a new entity gets added...
                item.Events.AddDelegate<UInt16>("changed:layer-depth", this.HandleItemLayerDepthChanged);
                this.UpdateItemLayer(item);

                return true;
            }

            return false;
        }

        public override bool Remove(Entity item)
        {
            if (base.Remove(item))
            { // When a entity gets removed...
                item.Events.RemoveDelegate<UInt16>("changed:layer-depth", this.HandleItemLayerDepthChanged);

                // Remove the entity from its old layer
                this.GetItemLayer(item)?.Entities.Remove(item);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Pull a new instance of the requested entity and
        /// automatically add it to the current collection.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="handle"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public TEntity Build<TEntity>(String handle, Action<TEntity> setup = null)
            where TEntity : Entity
        {
            var configuration = _entityLoader[handle];

            // Create the entity instance...
            var entity = configuration.Pool.Pull(_provider, (e) =>
            {
                e.Configuration = configuration;
                setup?.Invoke(e as TEntity);
            }) as TEntity;

            // Auto add the new entity...
            this.Add(entity);

            // return the new entity
            return entity;
        }

        private void UpdateItemLayer(Entity entity)
        {
            // Add the entity to its new layer...
            this.GetItemLayer(entity)?.Entities.Add(entity);
        }

        private Layer GetItemLayer(Entity entity)
        {
            return _layers.FirstOrDefault(l => l.Depth == entity.LayerDepth);
        }
        #endregion

        #region Event Handlers
        private void HandleItemLayerDepthChanged(object sender, UInt16 arg)
        {
            this.UpdateItemLayer(sender as Entity);
        }
        #endregion
    }
}
