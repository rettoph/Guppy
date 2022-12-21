using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Common.DependencyInjection
{
    public class AliasConfiguration
    {
        public Type Type { get; }
        public bool Filtered { get; set; }
        public int Order { get; set; }

        public AliasConfiguration(Type type)
        {
            this.Type = type;
            this.Filtered = true;
            this.Order = 0;
        }

        public AliasConfiguration SetFiltered(bool filtered)
        {
            this.Filtered = filtered;

            return this;
        }

        public AliasConfiguration SetOrder(int order)
        {
            this.Order = order;

            return this;
        }

        internal bool TryGetDescriptor(IServiceConfiguration service, [MaybeNullWhen(false)] out ServiceDescriptor descriptor)
        {
            if(this.Filtered == true)
            {
                descriptor = null;
                return false;
            }

            var type = service.ServiceType;
            object Factory(IServiceProvider provider)
            {
                return provider.GetRequiredService(type);
            }

            descriptor = ServiceDescriptorHelper.Describe(
                serviceType: this.Type,
                lifetime: service.Lifetime ?? ServiceLifetime.Transient,
                factory: Factory);

            return true;
        }
    }
}
