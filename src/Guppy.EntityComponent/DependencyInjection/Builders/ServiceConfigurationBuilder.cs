using Minnow.General.Interfaces;
using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public class ServiceConfigurationBuilder<TService> : IServiceConfigurationBuilder, IFluentPrioritizable<ServiceConfigurationBuilder<TService>>
        where TService : class
    {
        #region Protected Properties
        protected ServiceProviderBuilder services { get; private set; }
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public String Name { get; }

        /// <inheritdoc />
        public Int32 Priority { get; set; }

        /// <inheritdoc />
        public Type FactoryType { get; set; }

        /// <inheritdoc />
        public ServiceLifetime Lifetime { get; set; }

        /// <inheritdoc />
        public List<String> CacheNames { get; set; }
        #endregion

        #region Constructor
        public ServiceConfigurationBuilder(
            String name,
            ServiceProviderBuilder services)
        {
            this.services = services;

            this.Name = name;
            this.CacheNames = new List<String>();
            this.SetFactoryType<TService>();
        }
        #endregion

        #region SetFactoryType Methods
        /// <summary>
        /// Set the lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="factoryType"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> SetFactoryType(Type factoryType)
        {
            typeof(TService).ValidateAssignableFrom(factoryType);

            this.FactoryType = factoryType;

            return this;
        }
        /// <summary>
        /// Set the lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> SetFactoryType<TFactoryType>()
            where TFactoryType : class, TService
        {
            return this.SetFactoryType(typeof(TFactoryType));
        }
        #endregion

        #region RegisterTypeFactory Methods
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> RegisterTypeFactory<TFactoryType>(Type type, Action<TypeFactoryBuilder<TFactoryType>> builder)
            where TFactoryType : class, TService
        {
            TypeFactoryBuilder<TFactoryType> typeFactory = this.services.RegisterTypeFactory<TFactoryType>(type);
            builder(typeFactory);

            return this.SetFactoryType(typeFactory.Type);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> RegisterTypeFactory<TFactoryType>(Action<TypeFactoryBuilder<TFactoryType>> builder)
            where TFactoryType : class, TService
        {
            return this.RegisterTypeFactory<TFactoryType>(typeof(TFactoryType), builder);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> RegisterTypeFactory(Type type, Action<TypeFactoryBuilder<TService>> builder)
        {
            return this.RegisterTypeFactory<TService>(type, builder);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> RegisterTypeFactory(Action<TypeFactoryBuilder<TService>> builder)
        {
            return this.RegisterTypeFactory<TService>(builder);
        }
        #endregion

        #region SetLifetime Methods
        /// <summary>
        /// Set the service lifetime.
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> SetLifetime(ServiceLifetime lifetime)
        {
            this.Lifetime = lifetime;

            return this;
        }
        #endregion

        #region AddCacheName(s) Methods
        /// <summary>
        /// Add a singlular cache name.
        /// </summary>
        /// <param name="name">A value with which this service will be cached once activated.
        /// All queries matching  this value will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheName(String name)
        {
            this.CacheNames.Add(name);

            return this;
        }

        /// <summary>
        /// Add a singlular cache name, defaulting to the <see cref="Type.Name"/>
        /// </summary>
        /// <param name="type">A value with which this service will be cached once activated.
        /// All queries matching  this value will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheName(Type type)
        {
            return this.AddCacheName(type.Name);
        }

        /// <summary>
        /// Add many cache names at once.
        /// </summary>
        /// <param name="names">A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheNames(IEnumerable<String> names)
        {
            this.CacheNames.AddRange(names);

            return this;
        }

        /// <summary>
        /// Att many cache names at once, based on the input types.
        /// The names will default to <see cref="Type.Name"/>
        /// </summary>
        /// <param name="types">A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheNames(IEnumerable<Type> types)
        {
            return this.AddCacheNames(types.Select(t => t.FullName));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <paramref name="childType"/>
        /// </summary>
        /// <param name="childType"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheNamesBetweenTypes(Type parent, Type childType)
        {
            typeof(TService).ValidateAssignableFrom(childType);
            parent.ValidateAssignableFrom(childType);

            return this.AddCacheNames(childType.GetAncestors(parent));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <typeparamref name="TChild"/> 
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheNamesBetweenTypes<TParent, TChild>()
            where TChild : class, TService, TParent
        {
            return this.AddCacheNamesBetweenTypes(typeof(TParent), typeof(TChild));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <paramref name="childType"/>
        /// </summary>
        /// <param name="childType"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheNamesBetweenType(Type childType)
        {
            return this.AddCacheNamesBetweenTypes(typeof(TService), childType);
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <typeparamref name="TChild"/> 
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService> AddCacheNamesBetweenType<TChild>()
            where TChild : class, TService
        {
            return this.AddCacheNamesBetweenTypes<TService, TChild>();
        }
        #endregion

        #region TypeFactoryBuilder Implementation
        ServiceConfiguration IServiceConfigurationBuilder.Build(
            Dictionary<Type, TypeFactory> typeFactories,
            IEnumerable<CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>> allSetups)
        {

            Type factoryType = this.FactoryType ?? typeof(TService);
            TypeFactory typeFactory = typeFactories[factoryType];
            String[] cacheNames = this.CacheNames.Concat(this.Name).Distinct().ToArray();
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups = allSetups.Where(b => {
                return typeFactory.Type.IsAssignableToOrSubclassOfGenericDefinition(b.AssignableFactoryType) && b.Filter(this);
            }).ToArray();

            return this.Lifetime switch
            {
                ServiceLifetime.Singleton => new SingletonServiceConfiguration(this.Name, typeFactory, this.Lifetime, cacheNames, setups),
                ServiceLifetime.Scoped => new ScopedServiceConfiguration(this.Name, typeFactory, this.Lifetime, cacheNames, setups),
                _ => new TransientServiceConfiguration(this.Name, typeFactory, this.Lifetime, cacheNames, setups
                )
            };
        }
        #endregion
    }
}
