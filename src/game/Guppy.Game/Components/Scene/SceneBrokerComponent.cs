using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;

namespace Guppy.Engine.Components.Guppy
{
    internal class SceneBrokerComponent(IBrokerService brokers) : ISceneComponent<IScene>
    {
        private readonly IBrokerService _brokers = brokers;

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            _brokers.AddSubscribers<ISceneComponent>();
        }

        public void Dispose()
        {
            _brokers.RemoveSubscribers<ISceneComponent>();
        }
    }
}
