using Guppy.Common.Providers;
using Guppy.Common.Services;

namespace Guppy.Common.Implementations
{
    internal class BulkSubscriptionService : IBulkSubscriptionService
    {
        private readonly IBulkSubscriptionProvider[] _providers;

        public BulkSubscriptionService(IFiltered<IBulkSubscriptionProvider> providers)
        {
            _providers = providers.Instances.ToArray();
        }

        public void Subscribe(IEnumerable<object> instances)
        {
            foreach (var provider in _providers)
            {
                provider.Subscribe(instances);
            }
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            foreach (var provider in _providers)
            {
                provider.Unsubscribe(instances);
            }
        }
    }
}
