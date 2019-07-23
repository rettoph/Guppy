using Guppy.Collections;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.System;

namespace Guppy.Factories
{
    public class EntityFactory : InitializableFactory<Entity>
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

            // Add the configuration object to the args array...
            args = args.AddItems(configuration);

            // Create a new entity instance
            var entity = this.Create(_provider, configuration.Type, args) as Entity;

            // Return the new entity...
            return entity;
        }
    }
}
