﻿using Guppy.Configurations;
using Guppy.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;

namespace Guppy.Loaders
{
    public class EntityLoader : Loader<String, RegisteredEntityConfiguration, EntityConfiguration>
    {
        private IServiceProvider _provider;

        public EntityLoader(IServiceProvider provider, ILogger logger) : base(logger)
        {
            _provider = provider;

        }

        public void Register<TEntity>(String handle, String nameHandle, String descriptionHandle, IEntityData data = null, UInt16 priority = 0)
            where TEntity : Entity
        {
            this.Register(
                handle: handle,
                value: new RegisteredEntityConfiguration()
                {
                    NameHandle = nameHandle,
                    DescriptionHandle = descriptionHandle,
                    Data = data,
                    Type = typeof(TEntity)
                },
                priority: 1);
        }

        public override void Load()
        {
            base.Load();

            // Initialize all the custom entity configurations
            foreach (EntityConfiguration config in this.valuesTable.Select(kvp => kvp.Value))
                config.Data?.Initialize(_provider);
        }

        protected override Dictionary<String, EntityConfiguration> BuildValuesTable()
        {
            var stringLoader = _provider.GetLoader<StringLoader>();

            return this.registeredValuesList
                .GroupBy(rv => rv.Handle)
                .Select(g => g.OrderByDescending(rv => rv.Priority)
                    .FirstOrDefault())
                .ToDictionary(
                    keySelector: rv => rv.Handle,
                    elementSelector: rv => new EntityConfiguration(rv.Handle, rv.Value, stringLoader));
        }
    }
}
