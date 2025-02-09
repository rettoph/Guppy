using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Systems.Engine
{
    public class EngineBusSystem(IMessageBus bus) : IGlobalSystem, IUpdateSystem
    {
        private readonly IMessageBus _bus = bus;

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.PostUpdate)]
        public void Update(GameTime gameTime)
        {
            this._bus.Flush();
        }
    }
}