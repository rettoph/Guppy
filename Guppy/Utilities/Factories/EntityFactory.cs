using Guppy.Utilities.Loaders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Factories
{
    public class EntityFactory
    {
        private ILogger _logger;
        private EntityLoader _entities;
        private PooledFactory<Entity> _factory;

        public EntityFactory(ILogger logger, EntityLoader entities, PooledFactory<Entity> factory)
        {
            _logger = logger;
            _entities = entities;
            _factory = factory;
        }

        public TEntity Pull<TEntity>(String handle, Action<TEntity> setup = null)
            where TEntity : Entity
        {
            var configuration = _entities.GetValue(handle);

            if (configuration == null)
                throw new Exception($"Unknown entity handle => '{handle}'");

            _logger.LogTrace($"Pulling Entity<{configuration.Type.Name}>('{configuration.Handle}') instance...");

            return _factory.Pull<TEntity>(configuration.Type, e =>
            {
                e.Configuration = configuration;
                setup?.Invoke(e);
            });
        }

        public Entity Pull(String handle, Action<Entity> setup = null)
        {
            return this.Pull<Entity>(handle, setup);
        }
    }
}
