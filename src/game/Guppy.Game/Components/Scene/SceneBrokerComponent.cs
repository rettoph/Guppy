using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;

namespace Guppy.Engine.Components.Guppy
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    internal class SceneBrokerComponent : SceneComponent
    {
        private readonly IBrokerService _brokers;

        public SceneBrokerComponent(IBrokerService brokers)
        {
            _brokers = brokers;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _brokers.AddSubscribers<ISceneComponent>();
        }

        public void Dispose()
        {
            _brokers.RemoveSubscribers<ISceneComponent>();
        }
    }
}
