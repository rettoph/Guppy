using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Common.DependencyInjection
{
    public class AliasConfiguration
    {
        public Type Type { get; }
        public int Order { get; set; }

        public AliasConfiguration(Type type)
        {
            this.Type = type;
            this.Order = 0;
        }

        public AliasConfiguration SetOrder(int order)
        {
            this.Order = order;

            return this;
        }

        internal ServiceDescriptor GetDescriptor(IServiceConfiguration service)
        {
            var type = service.ServiceType;
            object Factory(IServiceProvider provider)
            {
                return provider.GetRequiredService(type);
            }

            var descriptor = ServiceDescriptorHelper.Describe(
                serviceType: this.Type,
                lifetime: service.Lifetime ?? ServiceLifetime.Transient,
                factory: Factory);

            return descriptor;
        }
    }
}
