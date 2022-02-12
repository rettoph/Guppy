using Minnow.General.Interfaces;
using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public class ServiceConfigurationBuilder<TService, TServiceConfigurationBuilder> : IServiceConfigurationBuilder, IFluentPrioritizable<TServiceConfigurationBuilder>
        where TService : class
        where TServiceConfigurationBuilder : ServiceConfigurationBuilder<TService, TServiceConfigurationBuilder>
    {
        #region Protected Properties
        protected ServiceProviderBuilder services { get; private set; }
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public Type FactoryType { get; set; }

        /// <inheritdoc />
        public ServiceLifetime Lifetime { get; set; }

        /// <inheritdoc />
        public HashSet<Type> Aliases { get; set; }

        /// <inheritdoc />
        public Int32 Priority { get; set; }
        #endregion

        #region Constructor
        public ServiceConfigurationBuilder(
            Type type,
            ServiceProviderBuilder services)
        {
            this.services = services;

            this.Type = type;
            this.Aliases = new HashSet<Type>();
            this.SetFactoryType(this.Type);
        }
        #endregion

        #region SetFactoryType Methods
        /// <summary>
        /// Set the lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="factoryType"></param>
        /// <returns></returns>
        public TServiceConfigurationBuilder SetFactoryType(Type factoryType)
        {
            this.Type.ValidateAssignableFrom(factoryType);
            this.FactoryType = factoryType;

            return this as TServiceConfigurationBuilder;
        }
        /// <summary>
        /// Set the lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <returns></returns>
        public TServiceConfigurationBuilder SetFactoryType<TFactoryType>()
            where TFactoryType : class, TService
        {
            return this.SetFactoryType(typeof(TFactoryType));
        }
        #endregion

        #region RegisterTypeFactory Methods
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="IServiceConfigurationBuilder"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TServiceConfigurationBuilder RegisterTypeFactory<TFactoryType>(Type type, Action<TypeFactoryBuilder<TFactoryType>> builder)
            where TFactoryType : class, TService
        {
            TypeFactoryBuilder<TFactoryType> typeFactory = this.services.RegisterTypeFactory<TFactoryType>(type);
            builder(typeFactory);

            return this.SetFactoryType(typeFactory.Type);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="IServiceConfigurationBuilder"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TServiceConfigurationBuilder RegisterTypeFactory<TFactoryType>(Action<TypeFactoryBuilder<TFactoryType>> builder)
            where TFactoryType : class, TService
        {
            return this.RegisterTypeFactory<TFactoryType>(typeof(TFactoryType), builder);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="IServiceConfigurationBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TServiceConfigurationBuilder RegisterTypeFactory(Type type, Action<TypeFactoryBuilder<TService>> builder)
        {
            return this.RegisterTypeFactory<TService>(type, builder);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="IServiceConfigurationBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TServiceConfigurationBuilder RegisterTypeFactory(Action<TypeFactoryBuilder<TService>> builder)
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
        public TServiceConfigurationBuilder SetLifetime(ServiceLifetime lifetime)
        {
            this.Lifetime = lifetime;

            return this as TServiceConfigurationBuilder;
        }
        #endregion

        #region AddAlias(es) Methods
        /// <summary>
        /// Add a singlular cache name.
        /// </summary>
        /// <param name="alias">A value with which this service will be cached once activated.
        /// All queries matching  this value will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public TServiceConfigurationBuilder AddAlias(Type alias)
        {
            alias.ValidateAssignableFrom(this.Type);
            this.Aliases.Add(alias);

            return this as TServiceConfigurationBuilder;
        }

        /// <summary>
        /// Add many cache names at once.
        /// </summary>
        /// <param name="aliases">A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public TServiceConfigurationBuilder AddAliases(IEnumerable<Type> aliases)
        {
            foreach(Type alias in aliases)
            {
                this.AddAlias(alias);
            }

            return this as TServiceConfigurationBuilder;
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <paramref name="childType"/>
        /// </summary>
        /// <param name="childType"></param>
        /// <returns></returns>
        public TServiceConfigurationBuilder AddAllAliases(Type baseType)
        {
            baseType.ValidateAssignableFrom(this.Type);

            return this.AddAliases(this.Type.GetAncestors(baseType));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <typeparamref name="TChild"/> 
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <returns></returns>
        public TServiceConfigurationBuilder AddAllAliases<TBaseType>()
        {
            return this.AddAllAliases(typeof(TBaseType));
        }
        #endregion

        #region TypeFactoryBuilder Implementation
        ServiceConfiguration IServiceConfigurationBuilder.Build(
            Dictionary<Type, TypeFactory> typeFactories,
            IEnumerable<CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>> allSetups)
        {

            Type factoryType = this.FactoryType ?? typeof(TService);
            TypeFactory typeFactory = typeFactories[factoryType];
            Type[] cacheNames = this.Aliases.Concat(this.Type).Distinct().ToArray();
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups = allSetups.Where(b => {
                return typeFactory.Type.IsAssignableToOrSubclassOfGenericDefinition(b.AssignableFactoryType) && b.Filter(this);
            }).ToArray();

            return this.Lifetime switch
            {
                ServiceLifetime.Singleton => new SingletonServiceConfiguration(this.Type, typeFactory, this.Lifetime, cacheNames, setups),
                ServiceLifetime.Scoped => new ScopedServiceConfiguration(this.Type, typeFactory, this.Lifetime, cacheNames, setups),
                _ => new TransientServiceConfiguration(this.Type, typeFactory, this.Lifetime, cacheNames, setups
                )
            };
        }
        #endregion
    }

    public class ServiceConfigurationBuilder<TService> : ServiceConfigurationBuilder<TService, ServiceConfigurationBuilder<TService>>
        where TService : class
    {
        public ServiceConfigurationBuilder(Type type, ServiceProviderBuilder services) : base(type, services)
        {
        }
    }
}
