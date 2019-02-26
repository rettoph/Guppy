﻿using Guppy.Collections;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class EntityFactory
    {
        private EntityLoader _entityLoader;
        private IServiceProvider _provider;

        public EntityFactory(EntityLoader entityLoader, IServiceProvider provider)
        {
            _entityLoader = entityLoader;
            _provider = provider;
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

            // Return the newly created entity
            return entity;
        }

        public Entity Create(String entityHandle, Guid id, params object[] args)
        {
            // Load the entity configuration
            var configuration = _entityLoader[entityHandle];

            // Create a new params array
            Array.Resize(ref args, args.Length + 1);
            args[args.Length - 1] = configuration;
            args[args.Length - 2] = id;

            var entity = ActivatorUtilities.CreateInstance(_provider, configuration.Type, args) as Entity;

            // Return the newly created entity
            return entity;
        }
    }
}
