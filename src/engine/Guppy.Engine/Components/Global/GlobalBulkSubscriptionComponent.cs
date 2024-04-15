using Guppy.Engine.Attributes;
using Guppy.Core.Messaging.Services;

namespace Guppy.Engine.Components.Global
{
    [AutoLoad]
    internal class GlobalBulkSubscriptionComponent : GlobalComponent, IDisposable
    {
        private readonly IMagicBrokerService _brokers;
        private IGlobalComponent[] _components;

        public GlobalBulkSubscriptionComponent(IMagicBrokerService brokers)
        {
            _brokers = brokers;
            _components = Array.Empty<IGlobalComponent>();
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _components = components;

            _brokers.Subscribe(_components);
        }

        public void Dispose()
        {
            _brokers.Unsubscribe(_components);
        }
    }
}
