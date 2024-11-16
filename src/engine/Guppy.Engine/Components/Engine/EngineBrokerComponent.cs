using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Components.Engine
{
    [AutoLoad]
    internal class EngineBrokerComponent(IBrokerService brokers) : IEngineComponent, IDisposable
    {
        private readonly IBrokerService _brokers = brokers;

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            _brokers.AddSubscribers<IEngineComponent>();
        }

        public void Dispose()
        {
            _brokers.RemoveSubscribers<IEngineComponent>();
        }
    }
}
