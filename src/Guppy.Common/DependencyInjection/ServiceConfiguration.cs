using Guppy.Common.Helpers;
using Guppy.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public class ServiceConfiguration<T> : IServiceConfiguration
        where T : class
    {
        private const ServiceLifetime DefaultLifetime = ServiceLifetime.Transient;

        private bool _dirty;
        private List<ServiceDescriptor> _descriptors;
        private readonly ServiceCollectionManager _manager;
        private readonly Dictionary<Type, AliasDescriptor> _aliases;

        public Type ServiceType { get; set; }

        public Type? ImplementationType { get; set; }

        public Type Type => this.ImplementationType ?? this.ServiceType;

        public ServiceLifetime? Lifetime { get; set; }

        public Func<IServiceProvider, T>? Factory { get; set; }

        public IReadOnlyDictionary<Type, AliasDescriptor> Aliases => _aliases;

        Func<IServiceProvider, object>? IServiceConfiguration.Factory => this.Factory;

        public ServiceConfiguration(ServiceCollectionManager manager)
        {
            _manager = manager;
            _aliases = new Dictionary<Type, AliasDescriptor>();
            _dirty = true;
            _descriptors = new List<ServiceDescriptor>();

            this.ServiceType = typeof(T);
        }


        public ServiceConfiguration<T> SetImplementationType(Type? implementationType)
        {
            _dirty = true;
            this.ImplementationType = implementationType;
            this.Factory = null;

            return this;
        }

        public ServiceConfiguration<T> SetImplementationType<TImplementation>()
            where TImplementation : class
        {
            return this.SetImplementationType(typeof(TImplementation));
        }

        public ServiceConfiguration<T> SetLifetime(ServiceLifetime? lifetime)
        {
            _dirty = true;
            this.Lifetime = lifetime;

            return this;
        }

        public ServiceConfiguration<T> SetInstance(T? instance)
        {
            _dirty = true;
            this.Factory = instance is null ? null : p => instance;

            return this;
        }

        public ServiceConfiguration<T> SetFactory(Func<IServiceProvider, T>? factory)
        {
            _dirty = true;
            this.Factory = factory;

            return this;
        }

        public ServiceConfiguration<T> AddAlias(Type alias, AliasType type = AliasType.Filtered)
        {
            ThrowIf.Type.IsNotAssignableFrom(alias, this.ServiceType);

            _dirty = true;

            if (_aliases.TryGetValue(alias, out var descriptor))
            {
                descriptor.Type = type;
                return this;
            }


            _aliases[alias] = new AliasDescriptor(alias, type);
            return this;
        }
        public ServiceConfiguration<T> AddAlias<TAlias>(AliasType type = AliasType.Filtered)
        {
            return this.AddAlias(typeof(TAlias), type);
        }

        public ServiceConfiguration<T> AddAliases(AliasType type = AliasType.Filtered, params Type[] aliases)
        {
            foreach(Type alias in aliases)
            {
                this.AddAlias(alias, type);
            }

            return this;
        }

        public ServiceConfiguration<T> AddInterfaceAliases(AliasType type = AliasType.Filtered)
        {
            foreach (Type alias in this.Type.GetInterfaces())
            {
                this.AddAlias(alias, type);
            }

            return this;
        }

        void IServiceCollectionManager.Refresh(IServiceCollection services)
        {
            if(!_dirty)
            {
                return;
            }

            foreach(ServiceDescriptor descriptor in _descriptors)
            {
                services.Remove(descriptor);
            }

            _descriptors.Clear();

            foreach (ServiceDescriptor descriptor in this.GetDescriptors())
            {
                services.Add(descriptor);
                _descriptors.Add(descriptor);
            }

            _dirty = false;
        }

        private IEnumerable<ServiceDescriptor> GetDescriptors()
        {
            yield return ServiceDescriptor.Singleton<IServiceConfiguration>(this);

            yield return ServiceDescriptorHelper.Describe(
                serviceType: this.ServiceType,
                lifetime: this.Lifetime ?? DefaultLifetime,
                implementationType: this.ImplementationType,
                factory: this.Factory);

            foreach (AliasDescriptor descriptor in this.GetAliasDescriptors(AliasType.Unfiltered))
            {
                yield return ServiceDescriptorHelper.Describe(
                    serviceType: descriptor.Alias,
                    lifetime: this.Lifetime ?? DefaultLifetime,
                    factory: this.UnfilteredAliasFactory);
            }
        }

        public IEnumerable<AliasDescriptor> GetAliasDescriptors(AliasType type)
        {
            return this.Aliases.Values.Where(x => x.Type == type);
        }

        private object UnfilteredAliasFactory(IServiceProvider provider)
        {
            return provider.GetRequiredService(this.ServiceType);
        }

        IServiceConfiguration IServiceConfiguration.SetImplementationType(Type? implementationType)
        {
            return this.SetImplementationType(implementationType);
        }

        IServiceConfiguration IServiceConfiguration.SetLifetime(ServiceLifetime? lifetime)
        {
            return this.SetLifetime(lifetime);
        }

        IServiceConfiguration IServiceConfiguration.SetInstance(object? instance)
        {
            if (instance is null)
            {
                return this.SetInstance(null);
            }

            if (instance is T casted)
            {
                return this.SetInstance(casted);
            }

            throw new ArgumentException();
        }

        IServiceConfiguration IServiceConfiguration.SetFactory(Func<IServiceProvider, object>? factory)
        {
            if (factory is null)
            {
                return this.SetFactory(null);
            }

            if (factory is Func<IServiceProvider, T> casted)
            {
                return this.SetFactory(casted);
            }

            throw new ArgumentException();
        }

        IServiceConfiguration IServiceConfiguration.AddAlias(Type alias, AliasType type = AliasType.Filtered)
        {
            return this.AddAlias(alias, type);
        }

        IServiceConfiguration IServiceConfiguration.AddAlias<TAlias>(AliasType type = AliasType.Filtered)
        {
            return this.AddAlias<TAlias>(type);
        }

        IServiceConfiguration IServiceConfiguration.AddAliases(AliasType type = AliasType.Filtered, params Type[] aliases)
        {
            return this.AddAliases(type, aliases);
        }

        IServiceConfiguration IServiceConfiguration.AddInterfaceAliases(AliasType type = AliasType.Filtered)
        {
            return this.AddInterfaceAliases(type);
        }

        IServiceConfiguration IServiceConfiguration.SetImplementationType<TImplementation>()
        {
            return this.SetImplementationType<TImplementation>();
        }
    }
}
