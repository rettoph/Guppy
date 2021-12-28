﻿using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using Guppy.EntityComponent.Interfaces;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public partial class ServiceProviderBuilder
    {
        #region Private Fields
        private List<ITypeFactoryBuilder> _typeFactories;
        private List<ICustomActionBuilder<TypeFactory, ITypeFactoryBuilder>> _builders;
        private List<ICustomActionBuilder<ServiceConfiguration, ServiceConfigurationBuilder>> _setups;
        private List<ServiceConfigurationBuilder> _serviceConfigurations;
        private List<ComponentConfigurationBuilder> _componentConfigurations;
        private List<ComponentFilterBuilder> _componentFilters;
        #endregion

        #region Constructors
        public ServiceProviderBuilder()
        {
            _typeFactories = new List<ITypeFactoryBuilder>();
            _builders = new List<ICustomActionBuilder<TypeFactory, ITypeFactoryBuilder>>();
            _setups = new List<ICustomActionBuilder<ServiceConfiguration, ServiceConfigurationBuilder>>();
            _serviceConfigurations = new List<ServiceConfigurationBuilder>();
            _componentConfigurations = new List<ComponentConfigurationBuilder>();
            _componentFilters = new List<ComponentFilterBuilder>();

            this.SetupIEntityConfiguration();
            this.SetupServiceConfiguration();
        }
        #endregion

        #region RegisterTypeFactory Methods
        public TypeFactoryBuilder<TFactory> RegisterTypeFactory<TFactory>(Type type)
            where TFactory : class
        {
            TypeFactoryBuilder<TFactory> typeFactory = new TypeFactoryBuilder<TFactory>(type);
            _typeFactories.Add(typeFactory);

            return typeFactory;
        }
        public TypeFactoryBuilder<TFactory> RegisterTypeFactory<TFactory>()
            where TFactory : class
        {
            return this.RegisterTypeFactory<TFactory>(typeof(TFactory));
        }
        public TypeFactoryBuilder<Object> RegisterTypeFactory(Type type)
        {
            return this.RegisterTypeFactory<Object>(type);
        }
        #endregion

        #region RegisterDefaultTypeFactory Methods
        public TypeFactoryBuilder<TFactory> RegisterDefaultTypeFactory<TFactory>()
            where TFactory : class, new()
        {
            return this.RegisterTypeFactory<TFactory>(typeof(TFactory))
                .SetMethod(p => new TFactory());
        }
        #endregion

        #region RegisterBuilder Methods
        /// <summary>
        /// Define a new builder action that will run immidiately after a <see cref="TypeFactory{T}"/> creates a new instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TypeFactory, ITypeFactoryBuilder> RegisterBuilder<T>(Type assignableFactoryType)
            where T : class
        {
            var builder = new CustomActionBuilder<T, TypeFactory, ITypeFactoryBuilder>(assignableFactoryType);
            _builders.Add(builder);

            return builder;
        }

        /// <summary>
        /// Define a new builder action that will run immidiately after a <see cref="TypeFactory{T}"/> creates a new instance.
        /// </summary>
        /// <typeparam name="TAssignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</typeparam>
        /// <returns></returns>
        public CustomActionBuilder<TAssignableFactoryType, TypeFactory, ITypeFactoryBuilder> RegisterBuilder<TAssignableFactoryType>()
            where TAssignableFactoryType : class
        {
            return this.RegisterBuilder<TAssignableFactoryType>(typeof(TAssignableFactoryType));
        }

        /// <summary>
        /// Define a new builder action that will run immidiately after a <see cref="TypeFactory{T}"/> creates a new instance.
        /// </summary>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<Object, TypeFactory, ITypeFactoryBuilder> RegisterBuilder(Type assignableFactoryType)
        {
            return this.RegisterBuilder<Object>(assignableFactoryType);
        }
        #endregion

        #region RegisterSetup Methods
        /// <summary>
        /// Define a new setup action that will run immidiately after a <see cref="TypeFactory{T}"/> selects a new instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, ServiceConfiguration, ServiceConfigurationBuilder> RegisterSetup<T>(Type assignableFactoryType)
            where T : class
        {
            var setup = new CustomActionBuilder<T, ServiceConfiguration, ServiceConfigurationBuilder>(assignableFactoryType);

            _setups.Add(setup);

            return setup;
        }

        /// <summary>
        /// Define a new setup action that will run immidiately after a <see cref="TypeFactory{T}"/> selects a new instance.
        /// </summary>
        /// <typeparam name="TAssignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</typeparam>
        /// <returns></returns>
        public CustomActionBuilder<TAssignableFactoryType, ServiceConfiguration, ServiceConfigurationBuilder> RegisterSetup<TAssignableFactoryType>()
            where TAssignableFactoryType : class
        {
            return this.RegisterSetup<TAssignableFactoryType>(typeof(TAssignableFactoryType));
        }

        /// <summary>
        /// Define a new setup action that will run immidiately after a <see cref="TypeFactory{T}"/> selects a new instance.
        /// </summary>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<Object, ServiceConfiguration, ServiceConfigurationBuilder> RegisterSetup(Type assignableFactoryType)
        {
            return this.RegisterSetup<Object>(assignableFactoryType);
        }
        #endregion

        #region RegisterService Methods
        public ServiceConfigurationBuilder<TService> RegisterService<TService>(String name)
            where TService : class
        {
            ServiceConfigurationBuilder<TService> serviceConfigurationBuilder = new ServiceConfigurationBuilder<TService>(name, this);
            _serviceConfigurations.Add(serviceConfigurationBuilder);

            return serviceConfigurationBuilder;
        }
        public ServiceConfigurationBuilder<TService> RegisterService<TService>()
            where TService : class
        {
            return this.RegisterService<TService>(typeof(TService).FullName);
        }
        public ServiceConfigurationBuilder<Object> RegisterService(String name)
        {
            return this.RegisterService<Object>(name);
        }
        #endregion

        //#region RegisterComponentService Methods
        //public ComponentServiceConfigurationBuilder<TComponent> RegisterComponentService<TComponent>(String name)
        //    where TComponent : class, IComponent
        //{
        //    ComponentServiceConfigurationBuilder<TComponent> serviceConfigurationBuilder = new ComponentServiceConfigurationBuilder<TComponent>(name, this);
        //    _serviceConfigurations.Add(serviceConfigurationBuilder);
        //
        //    return serviceConfigurationBuilder.SetLifetime(ServiceLifetime.Transient);
        //}
        //public ComponentServiceConfigurationBuilder<TComponent> RegisterComponentService<TComponent>()
        //    where TComponent : class, IComponent
        //{
        //    return this.RegisterComponentService<TComponent>(typeof(TComponent).FullName);
        //}
        //#endregion

        #region RegisterComponent Methods
        public ComponentConfigurationBuilder<TComponent> RegisterComponent<TComponent>(String componentName)
            where TComponent: class, IComponent
        {
            ComponentConfigurationBuilder<TComponent> componentConfiguration = new ComponentConfigurationBuilder<TComponent>(componentName, this);
            _componentConfigurations.Add(componentConfiguration);

            return componentConfiguration;
        }
        public ComponentConfigurationBuilder<IComponent> RegisterComponent(String componentName)
        {
            return this.RegisterComponent<IComponent>(componentName);
        }

        public ComponentConfigurationBuilder<TComponent> RegisterComponent<TComponent>()
            where TComponent : class, IComponent
        {
            return this.RegisterComponent<TComponent>(typeof(TComponent).FullName);
        }
        #endregion

        #region RegisterComponentTypeFilter Methods
        public ComponentFilterBuilder RegisterComponentFilter(Type assignableComponentType)
        {
            typeof(IComponent).ValidateAssignableFrom(assignableComponentType);

            ComponentFilterBuilder componentFilter = new ComponentFilterBuilder(assignableComponentType);
            _componentFilters.Add(componentFilter);

            return componentFilter;
        }

        public ComponentFilterBuilder RegisterComponentFilter<TAssignableComponent>()
            where TAssignableComponent : class, IComponent
        {
            ComponentFilterBuilder componentFilter = new ComponentFilterBuilder(typeof(TAssignableComponent));
            _componentFilters.Add(componentFilter);

            return componentFilter;
        }
        #endregion

        #region RegisterEntity Methods
        public EntityConfigurationBuilder<TEntity> RegisterEntity<TEntity>()
            where TEntity : class, IEntity
        {
            return new EntityConfigurationBuilder<TEntity>(this);
        }
        #endregion

        #region Protected Helper Methods
        private void BuildFactories(out Dictionary<Type, TypeFactory> factories)
        {
            // Build all BuilderActions & SetupActions...
            List<CustomAction<TypeFactory, ITypeFactoryBuilder>> allBuilders = _builders.Order().Select(b => b.Build()).ToList();

            // Build all TypeFactories...
            factories = _typeFactories.PrioritizeBy(f => f.Type)
                .Select(f => f.Build(allBuilders))
                .ToDictionaryByValue(
                    keySelector: f => f.Type);
        }

        private void BuildServiceConfigurations(
            Dictionary<Type, TypeFactory> factories,
            out DoubleDictionary<String, UInt32, ServiceConfiguration> serviceConfigurations)
        {
            List<CustomAction<ServiceConfiguration, ServiceConfigurationBuilder>> allSetups = _setups.Order().Select(b => b.Build()).ToList();

            // Build all ServiceConfigurations...
            List<ServiceConfiguration> allServiceConfigurations = _serviceConfigurations.PrioritizeBy(s => s.Name)
                .Select(s => s.Build(factories, allSetups))
                .ToList();

            serviceConfigurations = allServiceConfigurations.ToDoubleDictionary(sc => sc.Name, sc => sc.Id);
        }

        private void BuildEntityComponentConfigurations(
            DoubleDictionary<String, UInt32, ServiceConfiguration> serviceConfigurations,
            out Dictionary<UInt32, ComponentConfiguration[]> entityComponentConfigurations)
        {
            List<ComponentFilter> componentFilters = _componentFilters.Order().Select(cf => cf.Build()).ToList();
            List<ComponentConfiguration> componentConfigurations = _componentConfigurations.Order().Select(c => c.Build(componentFilters, serviceConfigurations)).ToList();
            List<(ServiceConfiguration entity, ComponentConfiguration[] components)> entityComponentConfigurationsList = new List<(ServiceConfiguration entity, ComponentConfiguration[] components)>();

            foreach (ServiceConfiguration service in serviceConfigurations.Values)
            {
                if (typeof(IEntity).IsAssignableFrom(service.TypeFactory.Type))
                {
                    entityComponentConfigurationsList.Add((
                        entity: service,
                        components: componentConfigurations.Where(cc => cc.EntityServiceConfigurations
                            .Any(ec => ec == service))
                            .ToArray() ?? Array.Empty<ComponentConfiguration>()
                    ));
                }
            }

            // Create lookup dictionary for entity configuration => 
            entityComponentConfigurations =  entityComponentConfigurationsList.ToDictionary(
                keySelector: tuple => tuple.entity.Id,
                elementSelector: tuple => tuple.components);
        }

        /// <summary>
        /// Construct a new <see cref="ServiceProvider"/> instance.
        /// </summary>
        /// <returns></returns>
        public ServiceProvider Build()
        {
            this.BuildFactories(out Dictionary<Type, TypeFactory> factories);
            this.BuildServiceConfigurations(factories, out DoubleDictionary<String, UInt32, ServiceConfiguration> serviceConfigurations);
            this.BuildEntityComponentConfigurations(serviceConfigurations, out Dictionary<UInt32, ComponentConfiguration[]> entityComponentConfigurations);

            return new ServiceProvider(
                serviceConfigurations,
                entityComponentConfigurations);
        }
        #endregion
    }
}
