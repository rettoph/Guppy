using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Systems.Scene
{
    public class SceneBusSystem(IMessageBus bus) : ISceneSystem, IUpdateSystem
    {
        private readonly IMessageBus _bus = bus;

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.PostUpdate)]
        public void Update(GameTime gameTime)
        {
            this._bus.Flush();
        }
    }
}