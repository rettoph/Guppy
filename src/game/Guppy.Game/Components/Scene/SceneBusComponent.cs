using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Guppy
{
    [AutoLoad]
    internal class SceneBusComponent(IBus bus) : ISceneComponent, IUpdatableComponent
    {
        private readonly IBus _bus = bus;

        [SequenceGroup<UpdateComponentSequenceGroup>(UpdateComponentSequenceGroup.PostUpdate)]
        public void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
