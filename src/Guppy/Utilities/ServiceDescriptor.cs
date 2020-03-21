using Guppy.Enums;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Utilities
{
    /// <summary>
    /// Defines a custom service setup, and will manage the
    /// instance lifetime.
    /// </summary>
    public sealed class ServiceDescriptor
    {
        #region Private Fields
        private Action<ServiceProvider, IService> _setup;
        #endregion

        #region Public Fields
        public readonly UInt32 Id;
        public readonly String Handle;
        public readonly Lifetime Lifetime;
        public readonly Type Type;
        public readonly Type BaseType;
        #endregion

        #region Constructor
        public ServiceDescriptor(String handle, Lifetime lifetime, Type type, Action<ServiceProvider, IService> setup)
            : this(handle, lifetime, type, type, setup)
        {

        }
        public ServiceDescriptor(String handle, Lifetime lifetime, Type type, Type baseType, Action<ServiceProvider, IService> setup)
        {
            _setup = setup;

            this.Id = xxHash.CalculateHash(Encoding.UTF8.GetBytes(handle));
            this.Handle = handle;
            this.Lifetime = lifetime;
            this.Type = type;
            this.BaseType = baseType;
        }
        #endregion

        #region Helper Methods
        public IService GetInstance(ServiceProvider provider, Action<ServiceProvider, IService> setup = null)
        {
            switch(this.Lifetime) {
                case Lifetime.Transient:
                    return this.Create(provider, setup);
                case Lifetime.Scoped:
                    return this.GetOrCreate(provider, provider.scopes, setup);
                case Lifetime.TypedScoped:
                    return this.GetOrCreate(provider, provider.typedScopes, setup);
                case Lifetime.Singleton:
                    return this.GetOrCreate(provider, provider.singletons, setup);
                case Lifetime.TypedSingleton:
                    return this.GetOrCreate(provider, provider.typedSingletons, setup);
                default:
                    throw new Exception($"Unknown or unhandled service lifetime: {this.Lifetime}");
            }
        }

        private IService Create(ServiceProvider provider, Action<ServiceProvider, IService> setup = null)
        {
            // Create instance & update meta data
            var instance = provider.BuildInstance(this.Type);
            instance.Id = Guid.NewGuid();
            instance.Descriptor = this;

            instance.TryPreInitialize(provider);

            _setup?.Invoke(provider, instance);
            setup?.Invoke(provider, instance);

            instance.TryInitialize(provider);
            instance.TryPostInitialize(provider);

            return instance;
        }

        private IService GetOrCreate(ServiceProvider provider, Dictionary<UInt32, IService> cache, Action<ServiceProvider, IService> setup = null)
        {
            if (!cache.ContainsKey(this.Id))
                cache.Add(this.Id, this.Create(provider, setup));

            return cache[this.Id];
        }

        private IService GetOrCreate(ServiceProvider provider, Dictionary<Type, IService> cache, Action<ServiceProvider, IService> setup = null)
        {
            if (!cache.ContainsKey(this.BaseType))
                this.Create(provider, (p, i) =>
                {
                    cache.Add(this.BaseType, i);
                    setup?.Invoke(p, i);
                });

            return cache[this.BaseType];
        }
        #endregion
    }
}
