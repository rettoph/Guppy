using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Components.Engine
{
    internal class EngineBrokerComponent(IBrokerService brokers) : IEngineComponent, IDisposable
    {
        private readonly IBrokerService _brokers = brokers;

        [SequenceGroup<InitializeComponentSequenceGroupEnum>(InitializeComponentSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            this._brokers.AddSubscribers<IEngineComponent>();
        }

        public void Dispose()
        {
            this._brokers.RemoveSubscribers<IEngineComponent>();
        }
    }
}