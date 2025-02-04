using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Systems;

namespace Guppy.Engine.Systems.Guppy
{
    public class SceneBrokerSystem(IBrokerService brokers) : ISceneSystem<IScene>
    {
        private readonly IBrokerService _brokers = brokers;
        private IScene _scene = null!;

        [SequenceGroup<InitializeSystemSequenceGroupEnum>(InitializeSystemSequenceGroupEnum.Initialize)]
        public void Initialize(IScene scene)
        {
            this._scene = scene;

            if (this._scene is IBaseSubscriber subscriber)
            {
                this._brokers.AddSubscribers([subscriber]);
            }
        }

        public void Dispose()
        {
            if (this._scene is IBaseSubscriber subscriber)
            {
                this._brokers.RemoveSubscribers([subscriber]);
            }
        }
    }
}