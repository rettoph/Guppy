using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Components;

namespace Guppy.Engine.Components.Global
{
    [AutoLoad]
    internal class EngineComponentBulkSubscriptionComponent : EngineComponent, IDisposable
    {
        private readonly IMagicBrokerService _brokers;
        private IFiltered<IEngineComponent> _components;

        public EngineComponentBulkSubscriptionComponent(IMagicBrokerService brokers, IFiltered<IEngineComponent> components)
        {
            _brokers = brokers;
            _components = components;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _brokers.Subscribe(_components);
        }

        public void Dispose()
        {
            _brokers.Unsubscribe(_components);
        }
    }
}
