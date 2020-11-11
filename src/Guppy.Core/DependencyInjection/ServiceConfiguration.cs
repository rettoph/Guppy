using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ServiceConfiguration : ServiceConfigurationDescriptor
    {
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
            IEnumerable<ServiceAction> actions) : base(descriptor.Name, descriptor.Lifetime, descriptor.Factory, descriptor.Priority)
        {
            this.Factory = factory;
            this.Actions = actions.GroupBy(a => a.Type).ToDictionary(
                keySelector: g => g.Key,
                elementSelector: g => g.ToArray());
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
        public Object GetInstance(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0)
        {
            switch (this.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    return this.BuildInstance(provider, setup, setupOrder, provider.CacheSingletonInstance);
                case ServiceLifetime.Scoped:
                    return this.BuildInstance(provider, setup, setupOrder, provider.CacheScopedInstance);
                case ServiceLifetime.Transient:
                    return this.BuildInstance(provider, setup, setupOrder);
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
        public Object BuildInstance(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null,
            Int32 setupOrder = 0,
            Action<Type, Object> cacher = null
        ) => this.Factory.Build(provider, cacher, this).Then(i =>
            {
                var ranCustom = setup == default;

                this.Actions[ServiceActionType.Setup].ForEach(s =>
                {
                    // Invoke the custom setup if neccessary...
                    if (!ranCustom && s.Order > setupOrder)
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
        #endregion
    }
}
