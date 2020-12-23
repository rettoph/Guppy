using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ServiceConfiguration : ServiceConfigurationDescriptor
    {
        #region Private Fields
        private Object _instance;
        #endregion

        #region Public Fields
        /// <summary>
        /// The factory to be used when creating a new instance of
        /// this service.
        /// </summary>
        public new readonly ServiceFactory Factory;

        /// <summary>
        /// A table of actions to preform on services within various
        /// positions of their lifecycle.
        /// </summary>
        public readonly Dictionary<ServiceActionType, ServiceAction[]> Actions;
        #endregion

        #region Constructors
        public ServiceConfiguration(
            ServiceConfigurationDescriptor descriptor,
            ServiceFactory factory,
            IEnumerable<ServiceAction> actions) : base(descriptor.Name, descriptor.Lifetime, descriptor.Factory, descriptor.Priority, descriptor.CacheType)
        {
            this.Factory = factory;
            this.Actions = this.Actions = DictionaryHelper.BuildEnumDictionary<ServiceActionType, ServiceAction[]>(
                with: actions.GroupBy(a => a.Type).ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.ToArray()),
                fallback: new ServiceAction[0]);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get or create a new instance of the described service
        /// based on the internal ServiceLifetime.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup">A custom setup action to preform.</param>
        /// <param name="setupOrder">The order in which to preform the custom setup method.</param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        public Object GetInstance(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0,
            Type[] generics = null)
        {
            switch (this.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    if (provider.TryGetSingletonInstance(this.CacheType, out _instance))
                        return _instance;

                    return this.BuildInstance(provider, setup, setupOrder, provider.CacheSingletonInstance, generics);
                case ServiceLifetime.Scoped:
                    if (provider.TryGetScopedInstance(this.CacheType, out _instance))
                        return _instance;

                    return this.BuildInstance(provider, setup, setupOrder, provider.CacheScopedInstance, generics);
                case ServiceLifetime.Transient:
                    return this.BuildInstance(provider, setup, setupOrder, null, generics);
            }

            throw new Exception("This shouldn't happen...");
        }

        /// <summary>
        /// Create a brand new instance of the descriped service
        /// regardless of the defined ServiceLifetime.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup">A custom setup action to preform.</param>
        /// <param name="setupOrder">The order in which to preform the custom setup method.</param>
        /// <param name="cacher">An automatic cache method excecuted immidiately after an instance is created or pulled from the pool.</param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        public Object BuildInstance(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0,
            Action<Type, Object> cacher = null,
            Type[] generics = null
        ) => this.Factory.Build(provider, this.CacheType, cacher, this, generics).Then(i =>
            {
                var ranCustom = setup == default;

                this.Actions[ServiceActionType.Setup].ForEach(s =>
                {
                    // Invoke the custom setup if neccessary...
                    if (!ranCustom && s.Order > setupOrder && (ranCustom = true))
                        setup.Invoke(i, provider, this);

                    // Invoke the internal setup method
                    s.Excecute(i, provider, this);
                });

                if (!ranCustom) // Invoke the custom setup if neccessary...
                    setup.Invoke(i, provider, this);
            });
        #endregion

        #region Static Helper Methods
        public static UInt32 GetId(String name)
            => name.xxHash();

        public static UInt32 GetId(Type type)
            => type.FullName.xxHash();

        public static UInt32 GetId<T>()
            => typeof(T).FullName.xxHash();
        #endregion
    }
}
