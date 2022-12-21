using Guppy.Common.DependencyInjection.Interfaces;
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
    public class ServiceConfiguration<TService> : IServiceConfiguration
        where TService : class
    {
        public const ServiceLifetime DefaultLifetime = ServiceLifetime.Transient;

        private bool _dirty;
        private List<ServiceDescriptor> _descriptors;

        public Type ServiceType { get; set; }

        public Type? ImplementationType { get; set; }

        public Type Type => this.ImplementationType ?? this.ServiceType;

        public ServiceLifetime? Lifetime { get; set; }

        public Func<IServiceProvider, TService>? Factory { get; set; }

        public AliasesConfiguration Aliases { get; }

        Func<IServiceProvider, object>? IServiceConfiguration.Factory => this.Factory;

        public ServiceConfiguration()
        {
            _dirty = true;
            _descriptors = new List<ServiceDescriptor>();

            this.ServiceType = typeof(TService);
            this.Aliases = new AliasesConfiguration(this);
        }


        public ServiceConfiguration<TService> SetImplementationType(Type? implementationType)
        {
            _dirty = true;
            this.ImplementationType = implementationType;
            this.Factory = null;

            return this;
        }

        public ServiceConfiguration<TService> SetImplementationType<TImplementation>()
            where TImplementation : class
        {
            return this.SetImplementationType(typeof(TImplementation));
        }

        public ServiceConfiguration<TService> SetLifetime(ServiceLifetime? lifetime)
        {
            _dirty = true;
            this.Lifetime = lifetime;

            return this;
        }

        public ServiceConfiguration<TService> SetInstance(TService? instance)
        {
            _dirty = true;
            this.Factory = instance is null ? null : p => instance;

            return this;
        }

        public ServiceConfiguration<TService> SetFactory(Func<IServiceProvider, TService>? factory)
        {
            _dirty = true;
            this.Factory = factory;

            return this;
        }

        public ServiceConfiguration<TService> AddAlias(Type type, Action<AliasConfiguration>? configure = null)
        {
            ThrowIf.Type.IsNotAssignableFrom(type, this.ServiceType);

            _dirty = true;
            var alias = new AliasConfiguration(type);
            configure?.Invoke(alias);

            this.Aliases.Add(alias);

            return this;
        }
        public ServiceConfiguration<TService> AddAlias<TAlias>(Action<AliasConfiguration>? configure = null)
        {
            return this.AddAlias(typeof(TAlias), configure);
        }

        public ServiceConfiguration<TService> AddAliases(Action<AliasConfiguration>? configure = null, params Type[] types)
        {
            foreach(Type alias in types)
            {
                this.AddAlias(alias, configure);
            }

            return this;
        }

        public ServiceConfiguration<TService> AddInterfaceAliases(Action<AliasConfiguration>? configure = null)
        {
            foreach (Type alias in this.Type.GetInterfaces().Distinct())
            {
                this.AddAlias(alias, configure);
            }

            return this;
        }

        void IServiceConfiguration.Refresh(IServiceCollection services)
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

            foreach (ServiceDescriptor descriptor in this.Aliases.GetDescriptors())
            {
                yield return descriptor;
            }
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

            if (instance is TService casted)
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

            if (factory is Func<IServiceProvider, TService> casted)
            {
                return this.SetFactory(casted);
            }

            throw new ArgumentException();
        }

        IServiceConfiguration IServiceConfiguration.AddAlias(Type alias, Action<AliasConfiguration>? configure = null)
        {
            return this.AddAlias(alias, configure);
        }

        IServiceConfiguration IServiceConfiguration.AddAlias<TAlias>(Action<AliasConfiguration>? configure = null)
        {
            return this.AddAlias<TAlias>(configure);
        }

        IServiceConfiguration IServiceConfiguration.AddAliases(Action<AliasConfiguration>? configure = null, params Type[] aliases)
        {
            return this.AddAliases(configure, aliases);
        }

        IServiceConfiguration IServiceConfiguration.AddInterfaceAliases(Action<AliasConfiguration>? configure = null)
        {
            return this.AddInterfaceAliases(configure);
        }

        IServiceConfiguration IServiceConfiguration.SetImplementationType<TImplementation>()
        {
            return this.SetImplementationType<TImplementation>();
        }
    }
}
