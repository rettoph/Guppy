using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Microsoft.Extensions.Logging;

namespace Guppy.Factories
{
    public sealed class EntityFactory : InitializableFactory<Entity>
    {
        #region Private Fields
        private EntityLoader _loader;
        #endregion

        #region Constructor
        public EntityFactory(EntityLoader loader, IPoolManager<Entity> pools, IServiceProvider provider) : base(pools, provider)
        {
            _loader = loader;
        }
        #endregion

        public TEntity Build<TEntity>(String handle, Action<TEntity> setup = null)
            where TEntity : Entity
        {
            var configuration = _loader[handle];

            this.logger.LogTrace($"EntityFactory => Building Entity<{configuration.Type.Name}>('{configuration.Handle}') instance...");

            return this.Build<TEntity>(configuration.Type, e =>
            {
                e.Configuration = configuration;

                setup?.Invoke(e);
            });
        }
    }
}
