using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Components;

namespace Guppy.Engine.Components.Global
{
    [AutoLoad]
    internal class EngineBrokerComponent : EngineComponent, IDisposable
    {
        private readonly IBrokerService _brokers;

        public EngineBrokerComponent(IBrokerService brokers)
        {
            _brokers = brokers;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _brokers.AddSubscribers<IEngineComponent>();
        }

        public void Dispose()
        {
            _brokers.RemoveSubscribers<IEngineComponent>();
        }
    }
}
