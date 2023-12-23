using Guppy.Attributes;
using Guppy.Common.Services;

namespace Guppy.Components.Global
{
    [AutoLoad]
    internal class GlobalBulkSubscriptionComponent : GlobalComponent, IDisposable
    {
        private readonly IBulkSubscriptionService _subscriptions;
        private IGlobalComponent[] _components;

        public GlobalBulkSubscriptionComponent(IBulkSubscriptionService subscriptions)
        {
            _subscriptions = subscriptions;
            _components = Array.Empty<IGlobalComponent>();
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components;

            _subscriptions.Subscribe(_components);
        }

        public void Dispose()
        {
            _subscriptions.Unsubscribe(_components);
        }
    }
}
