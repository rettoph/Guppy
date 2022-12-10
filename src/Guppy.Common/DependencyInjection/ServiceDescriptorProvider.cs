using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public class ServiceDescriptorProvider : IServiceDescriptorProvider
    {
        private IDictionary<Type, ServiceDescriptor> _services;

        public ServiceDescriptorProvider()
        {
            _services = new Dictionary<Type, ServiceDescriptor>();
        }

        public ServiceConfiguration<T> AddService<T>()
            where T : class
        {
            return this.AddService<T>(typeof(T));
        }

        public ServiceConfiguration<object> AddService(Type type)
        {
            return this.AddService<object>(type);
        }

        private ServiceConfiguration<T> AddService<T>(Type type)
            where T : class
        {
            ThrowIf.Type.IsNotAssignableFrom<T>(type);

            var configuration = new ServiceConfiguration<T>(type);
            _providers.Add(configuration);

            return configuration;
        }

        public IEnumerable<ServiceDescriptor> GetDescriptors()
        {
            return _providers.SelectMany(x => x.GetDescriptors());
        }
    }
}
