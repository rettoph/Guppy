using Guppy.Collections;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class EntityFactory
    {
        private EntityLoader _entityLoader;
        private IServiceProvider _provider;
        private ILogger _logger;

        public EntityFactory(EntityLoader entityLoader, IServiceProvider provider)
        {
            _entityLoader = entityLoader;
            _provider = provider;
            _logger = _provider.GetService<ILogger>();
        }

        public Entity Create(String entityHandle, params object[] args)
        {
            // Load the entity configuration
            var configuration = _entityLoader[entityHandle];

            // Create a new params array
            Array.Resize(ref args, args.Length + 1);
            args[args.Length - 1] = configuration;

            // Create a new entity instance
            var entity = ActivatorUtilities.CreateInstance(_provider, configuration.Type, args) as Entity;

            _logger.LogDebug($"Created new Entity<{entity.GetType().Name}>({entity.Id})");

            // Return the newly created entity
            return entity;
        }

        public Entity Create(String entityHandle, Guid id, params object[] args)
        {
            // Load the entity configuration
            var configuration = _entityLoader[entityHandle];

            // Create a new params array
            Array.Resize(ref args, args.Length + 2);
            args[args.Length - 1] = configuration;
            args[args.Length - 2] = id;

            var entity = ActivatorUtilities.CreateInstance(_provider, configuration.Type, args) as Entity;

            _logger.LogDebug($"Created new Entity<{entity.GetType().Name}>({entity.Id})");

            // Return the newly created entity
            return entity;
        }
    }
}
