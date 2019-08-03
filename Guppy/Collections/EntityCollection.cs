using Guppy.Extensions.DependencyInjection;
using Guppy.Utilities.Loaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class EntityCollection : ReusableCollection<Entity>
    {
        #region Private Fields
        private IServiceProvider _provider;
        private EntityLoader _entityLoader;
        #endregion

        #region Constructors
        public EntityCollection(EntityLoader entityLoader, IServiceProvider provider) : base(provider)
        {
            _entityLoader = entityLoader;
            _provider = provider;
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
        #endregion
    }
}
