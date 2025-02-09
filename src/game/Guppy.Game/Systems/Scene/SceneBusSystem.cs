using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Systems.Scene
{
    public class SceneBusSystem(IMessageBus bus, IScene scene) : ISceneSystem, IInitializeSystem, IDeinitializeSystem, IUpdateSystem
    {
        private readonly IMessageBus _bus = bus;
        private readonly IScene _scene = scene;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize()
        {
            this._bus.Subscribe(this._scene);
        }

        [SequenceGroup<DeinitializeSequenceGroupEnum>(DeinitializeSequenceGroupEnum.PreInitialize)]
        public void Deinitialize()
        {
            this._bus.Unsubscribe(this._scene);
        }

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.PostUpdate)]
        public void Update(GameTime gameTime)
        {
            this._bus.Flush();
        }
    }
}