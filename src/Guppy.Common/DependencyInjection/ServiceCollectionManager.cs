using Guppy.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    internal sealed class ServiceCollectionManager : IServiceCollectionManager
    {
        private readonly IDictionary<Type, IList<IServiceConfiguration>> _services;

        public ServiceCollectionManager()
        {
            _services = new Dictionary<Type, IList<IServiceConfiguration>>();
        }

        public IServiceConfiguration AddService(Type serviceType)
        {
            if(!_services.TryGetValue(serviceType, out var configurations))
            {
                configurations = new List<IServiceConfiguration>();
                _services.Add(serviceType, configurations);
            }

            Type configurationType = typeof(ServiceConfiguration<>).MakeGenericType(serviceType);
            var configuration = (IServiceConfiguration)Activator.CreateInstance(configurationType, new object[] { })!;
            configurations.Add(configuration);

            return configuration;
        }

        public IServiceConfiguration GetService(Type serviceType, Func<IServiceConfiguration, bool> predicate)
        {
            if (!_services.TryGetValue(serviceType, out var configurations))
            {
                return this.AddService(serviceType);
            }

            return configurations.First(predicate);
        }

        public IServiceConfiguration GetService(Type serviceType)
        {
            if (!_services.TryGetValue(serviceType, out var configurations))
            {
                return this.AddService(serviceType);
            }

            return configurations.First();
        }

        public void Refresh(IServiceCollection services)
        {
            foreach(IServiceConfiguration service in _services.Values.SelectMany(x => x))
            {
                service.Refresh(services);
            }
        }
    }
}
