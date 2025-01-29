using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;

namespace Guppy.Engine.Components.Guppy
{
    public class SceneBrokerComponent(IBrokerService brokers) : ISceneComponent<IScene>
    {
        private readonly IBrokerService _brokers = brokers;

        [SequenceGroup<InitializeComponentSequenceGroupEnum>(InitializeComponentSequenceGroupEnum.Initialize)]
        public void Initialize(IScene scene)
        {
            this._brokers.AddSubscribers<ISceneComponent>();
        }

        public void Dispose()
        {
            this._brokers.RemoveSubscribers<ISceneComponent>();
        }
    }
}