using Guppy.Attributes;
using Guppy.Extensions;
using Guppy.Utilities.Configurations;
using Guppy.Utilities.Options;
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
        private PoolLoader _pools;
        private StringLoader _strings;

        public EntityLoader(PoolLoader pools, StringLoader strings, ILogger logger) : base(logger)
        {
            _pools = pools;
            _strings = strings;
        }

        /// <summary>
        /// Register a new entity type, alowing the particular entity to be
        /// created from the get-go
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="nameHandle"></param>
        /// <param name="descriptionHandle"></param>
        /// <param name="customData"></param>
        public void TryRegister<TEntity>(String handle, String nameHandle = "name:entity:default", String descriptionHandle = "description:entity:default", Object customData = null, UInt16 priority = 100)
            where TEntity : Entity
        {
            this.logger.LogTrace($"Registering new Entity<{typeof(TEntity).Name}>({priority}) => '{handle}'");

            // Ensure a pool for the particular entity exists.
            _pools.TryRegisterInitializable<TEntity>(priority);

            base.Register(handle, new EntityConfiguration()
            {
                Handle = handle,
                Name = nameHandle,
                Description = descriptionHandle,
                CustomData = customData,
                Type = typeof(TEntity)
            }, priority);
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
