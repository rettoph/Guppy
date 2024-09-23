using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Components.Global
{
    [AutoLoad]
    [SequenceGroup<InitializeSequence>(InitializeSequence.Initialize)]
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
