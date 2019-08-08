using Guppy.Attributes;
using Guppy.Configurations;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Loaders
{
    [IsLoader(110)]
    public class EntityLoader : Loader<String, EntityConfiguration, EntityConfiguration>
    {
        private Dictionary<Type, UniquePool<Entity>> _pools;
        private StringLoader _strings;

        public EntityLoader(StringLoader stringLoader, ILogger logger) : base(logger)
        {
            _strings = stringLoader;
            _pools = new Dictionary<Type, UniquePool<Entity>>();
        }

        /// <summary>
        /// Register a new entity type, alowing the particular entity to be
        /// created from the get-go
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="nameHandle"></param>
        /// <param name="descriptionHandle"></param>
        /// <param name="data"></param>
        public void TryRegister<TEntity>(String handle, String nameHandle = "name:entity:default", String descriptionHandle = "description:entity:default", Object data = null, UInt16 priority = 100)
            where TEntity : Entity
        {
            this.logger.LogDebug($"Registering new Entity<{typeof(TEntity).Name}>({priority}) => '{handle}'");

            // Ensure that a pool for the requested entity exists...
            if (!_pools.ContainsKey(typeof(TEntity)))
                _pools[typeof(TEntity)] = new UniquePool<Entity>(typeof(TEntity));

            // Register the entities configuration...
            base.Register(handle, new EntityConfiguration()
            {
                Handle = handle,
                Name = nameHandle,
                Description = descriptionHandle,
                Data = data,
                Type = typeof(TEntity),
                Pool = _pools[typeof(TEntity)]
            },
            priority);
        }

        protected override Dictionary<String, EntityConfiguration> BuildValuesTable()
        {
            return this.registeredValuesList
                .GroupBy(rv => rv.Handle)
                .Select(g => g.OrderByDescending(rv => rv.Priority)
                    .FirstOrDefault())
                .ToDictionary(
                    keySelector: rv => rv.Handle,
                    elementSelector: rv =>
                    {
                        // Update the name & description based on saved string data...
                        rv.Value.Name = _strings[rv.Value.Name];
                        rv.Value.Description = _strings[rv.Value.Description];

                        return rv.Value;
                    });
        }
    }
}
