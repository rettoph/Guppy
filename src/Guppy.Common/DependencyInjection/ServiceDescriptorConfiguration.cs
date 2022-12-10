using Guppy.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public abstract class ServiceDescriptorConfiguration : IServiceDescriptorProvider
    {
        public abstract Type ServiceType { get; }

        public abstract Type? ImplementationType { get; }

        public Type Type => this.ImplementationType ?? this.ServiceType;

        public abstract ServiceLifetime Lifetime { get; }

        public abstract object? Instance { get; }

        public abstract Func<IServiceProvider, object>? Factory { get; }

        public abstract IReadOnlyDictionary<Type, AliasDescriptor> Aliases { get; }

        public IEnumerable<AliasDescriptor> GetAliasDescriptors(AliasType type)
        {
            return this.Aliases.Values.Where(x => x.Type == type);
        }

        internal abstract IEnumerable<ServiceDescriptor> GetDescriptors();

        IEnumerable<ServiceDescriptor> IServiceDescriptorProvider.GetDescriptors()
        {
            return this.GetDescriptors();
        }
    }

    public sealed class ServiceConfiguration<T> : ServiceDescriptorConfiguration
        where T : class
    {
        private readonly Dictionary<Type, AliasDescriptor> _aliases;
        private readonly Type _serviceType;
        private Type? _implementationType;
        private ServiceLifetime _lifetime;
        private T? _instance;
        private Func<IServiceProvider, T>? _factory;

        public override Type ServiceType => _serviceType;

        public override Type? ImplementationType => _implementationType;

        public override ServiceLifetime Lifetime => _lifetime;

        public override T? Instance => _instance;

        public override Func<IServiceProvider, T>? Factory => _factory;

        public override IReadOnlyDictionary<Type, AliasDescriptor> Aliases => _aliases;

        internal ServiceConfiguration(Type type)
        {
            _aliases = new Dictionary<Type, AliasDescriptor>();
            _serviceType = type;
            _lifetime = ServiceLifetime.Transient;
        }

        public ServiceConfiguration<T> SetImplementationType(Type? implementationType)
        {
            _implementationType = implementationType;
            _factory = null;
            _instance = null;

            return this;
        }

        public ServiceConfiguration<T> SetLifetime(ServiceLifetime lifetime)
        {
            _lifetime = lifetime;

            return this;
        }

        public ServiceConfiguration<T> SetInstance(T? instance)
        {
            if (instance is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom(this.Type, instance.GetType());
            }

            _lifetime = ServiceLifetime.Singleton;
            _implementationType = null;
            _factory = null;
            _instance = instance;

            return this; ;
        }

        public ServiceConfiguration<T> SetFactory(Func<IServiceProvider, T>? factory)
        {
            _implementationType = null;
            _factory = factory;
            _instance = null;

            return this;
        }

        public ServiceConfiguration<T> WithAlias(AliasType type, Type alias)
        {
            ThrowIf.Type.IsNotAssignableFrom(alias, this.ServiceType);

            if(_aliases.TryGetValue(alias, out var descriptor))
            {
                descriptor.Type = type;
                return this;
            }


            _aliases[alias] = new AliasDescriptor(alias, type);
            return this;
        }
        public ServiceConfiguration<T> WithAlias<TAlias>(AliasType type)
        {
            return this.WithAlias(type, typeof(TAlias));
        }

        public ServiceConfiguration<T> WithAliases(AliasType type, params Type[] aliases)
        {
            foreach(Type alias in aliases)
            {
                this.WithAlias(type, alias);
            }

            return this;
        }

        public ServiceConfiguration<T> WithInterfaceAliases(AliasType type)
        {
            foreach (Type alias in this.Type.GetInterfaces())
            {
                this.WithAlias(type, alias);
            }

            return this;
        }

        internal override IEnumerable<ServiceDescriptor> GetDescriptors()
        {
            yield return ServiceDescriptor.Singleton<ServiceDescriptorConfiguration>(this);

            yield return ServiceDescriptorHelper.Describe(
                serviceType: this.ServiceType, 
                lifetime: this.Lifetime,
                implementationType: this.ImplementationType,
                factory: this.Factory,
                instance: this.Instance);

            foreach(AliasDescriptor descriptor in this.GetAliasDescriptors(AliasType.Unfiltered))
            {
                yield return ServiceDescriptorHelper.Describe(
                    serviceType: descriptor.Alias,
                    lifetime: this.Lifetime,
                    factory: this.UnfilteredAliasFactory);
            }
        }

        private object UnfilteredAliasFactory(IServiceProvider provider)
        {
            return provider.GetRequiredService(this.ServiceType);
        }
    }
}
