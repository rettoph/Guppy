using Guppy.DependencyInjection.Structs;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Simple class used to describe a specific service.
    /// Note: Services are indexed by their Id
    /// </summary>
    public sealed class ServiceDescriptor
    {
        #region Public Attributes
        /// <summary>
        /// A unique index calculated based on a hash value 
        /// of the recieved name.
        /// </summary>
        public readonly UInt32 Id;

        /// <summary>
        /// A unique human readable identifier for the current
        /// service.
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// The internal factory used to generate a fresh instance of
        /// the current service.
        /// </summary>
        public readonly ServiceFactory Factory;

        /// <summary>
        /// The current service's lifetime.
        /// </summary>
        public readonly ServiceLifetime Lifetime;

        /// <summary>
        /// The internally stored type used when the service lifetime
        /// is scoped or singleton. If null, then the cache type will
        /// default to the recieved factory service type.
        /// </summary>
        public readonly Type CacheType;

        /// <summary>
        /// When true the provider will automatically generate an instance
        /// of this service on creation. 
        /// 
        /// (This is ignored for transient lifetimes)
        /// </summary>
        public readonly Boolean AutoBuild;

        /// <summary>
        /// The configurations to run when returning a new instance of this
        /// service.
        /// </summary>
        public readonly ServiceConfiguration[] Configurations;
        #endregion

        #region Constructor
        internal ServiceDescriptor(ServiceDescriptorData data, ServiceProvider provider, ServiceConfiguration[] configurations)
        {
            this.Id = ServiceDescriptor.GetId(data.Name);
            this.Name = data.Name;
            this.Factory = provider.GetFactory(data.Factory);
            this.Lifetime = data.Lifetime;
            this.CacheType = data.CacheType ?? this.Factory.Type;
            this.AutoBuild = data.AutoBuild;
            this.Configurations = configurations;
        }
        #endregion

        #region Helper Methods
        public Object Build(ServiceProvider provider, Action<Object, ServiceProvider, ServiceDescriptor> setup = null, Action<Object> cacher = null)
        {
            var instance = this.Factory.Build(provider);
            cacher?.Invoke(instance);

            var ranSetup = setup == null;
            this.Configurations.ForEach(c =>
            {
                if (!ranSetup && c.Order >= 0 && (ranSetup = true))
                    setup.Invoke(instance, provider, this);

                c.Configure(instance, provider, this);
            });

            if (!ranSetup)
                setup.Invoke(instance, provider, this);

            return instance;
        }

        /// <summary>
        /// Returns an instance of the current service based on the lifetime.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public Object GetInstance(ServiceProvider provider, Action<Object, ServiceProvider, ServiceDescriptor> setup = null)
        {
            switch (this.Lifetime)
            {
                case ServiceLifetime.Transient:
                    return this.Build(provider, setup);
                case ServiceLifetime.Scoped:
                    var scope = provider.GetScope();
                    if (!scope.scopedInstances.ContainsKey(this.CacheType))
                        this.Build(scope, setup, i => scope.scopedInstances[this.CacheType] = i);
                    return scope.scopedInstances[this.CacheType];
                case ServiceLifetime.Singleton:
                    if (!provider.root.singletonInstances.ContainsKey(this.CacheType))
                        this.Build(provider.root, setup, i => provider.root.singletonInstances[this.CacheType] = i);
                    return provider.root.singletonInstances[this.CacheType];
            }

            throw new Exception("This should never happen.");
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Automatically convert a recieved service name into 
        /// the corresponding service ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static UInt32 GetId(String name)
            => name.xxHash();

        public static UInt32 GetId(Type type)
            => ServiceDescriptor.GetId(type.FullName);
        #endregion
    }
}
