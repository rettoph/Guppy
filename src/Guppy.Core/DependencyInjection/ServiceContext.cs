using Guppy.DependencyInjection.Structs;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
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
    public sealed class ServiceContext
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
        internal ServiceContext(ServiceContextData data, ServiceProvider provider, ServiceConfiguration[] configurations)
        {
            this.Id = ServiceContext.GetId(data.Name);
            this.Name = data.Name;
            this.Factory = provider.GetFactory(data.Factory);
            this.Lifetime = data.Lifetime ?? this.Factory.Descriptor.Lifetime;
            this.AutoBuild = data.AutoBuild;
            this.Configurations = configurations;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Return a new instance of the current service This will
        /// completely ignore scope.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup"></param>
        /// <param name="cacher"></param>
        /// <returns></returns>
        public Object Build(ServiceProvider provider, Action<Object, ServiceProvider, ServiceContext> setup = null, Action<Object> cacher = null)
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
        public Object GetInstance(ServiceProvider provider, Action<Object, ServiceProvider, ServiceContext> setup = null)
        {
            switch (this.Lifetime)
            {
                case ServiceLifetime.Transient:
                    return this.Build(provider, setup);
                case ServiceLifetime.Scoped:
                    var scope = provider.GetScope();
                    if (!scope.scopedInstances.ContainsKey(this.Factory.Descriptor.ServiceType))
                        this.Build(scope, setup, i => scope.scopedInstances[this.Factory.Descriptor.ServiceType] = i);
                    return scope.scopedInstances[this.Factory.Descriptor.ServiceType];
                case ServiceLifetime.Singleton:
                    if (!provider.root.singletonInstances.ContainsKey(this.Factory.Descriptor.ServiceType))
                        this.Build(provider.root, setup, i => provider.root.singletonInstances[this.Factory.Descriptor.ServiceType] = i);
                    return provider.root.singletonInstances[this.Factory.Descriptor.ServiceType];
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
            => ServiceContext.GetId(type.FullName);
        #endregion
    }
}
