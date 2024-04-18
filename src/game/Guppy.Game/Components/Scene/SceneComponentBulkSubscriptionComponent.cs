using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;

namespace Guppy.Engine.Components.Guppy
{
    [AutoLoad]
    internal class SceneComponentBulkSubscriptionComponent : SceneComponent
    {
        private readonly IGameEngine _engine;
        private readonly IMagicBrokerService _brokers;
        private IFiltered<ISceneComponent> _components;

        public SceneComponentBulkSubscriptionComponent(IMagicBrokerService brokers, IFiltered<ISceneComponent> components, IGameEngine engine)
        {
            _engine = engine;
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
