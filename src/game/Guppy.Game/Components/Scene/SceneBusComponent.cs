using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Guppy
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    [Sequence<UpdateSequence>(UpdateSequence.PostUpdate)]
    internal class SceneBusComponent : SceneComponent, IGuppyUpdateable
    {
        private readonly IBus _bus;

        public SceneBusComponent(IBus bus)
        {
            _bus = bus;
        }

        public void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
