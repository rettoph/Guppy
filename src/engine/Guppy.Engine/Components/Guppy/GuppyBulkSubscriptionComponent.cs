using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;

namespace Guppy.Engine.Components.Guppy
{
    [AutoLoad]
    internal class GuppyBulkSubscriptionComponent : GuppyComponent
    {
        private readonly IMagicBrokerService _brokers;
        private IGuppyComponent[] _components;

        public GuppyBulkSubscriptionComponent(IMagicBrokerService brokers)
        {
            _brokers = brokers;
            _components = Array.Empty<IGuppyComponent>();
        }

        public override void Initialize(IGuppy guppy)
        {
            base.Initialize(guppy);

            _components = guppy.Components;

            _brokers.Subscribe(_components);
        }

        public void Dispose()
        {
            _brokers.Unsubscribe(_components);
        }
    }
}
