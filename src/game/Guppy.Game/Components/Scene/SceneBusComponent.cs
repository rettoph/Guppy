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
    internal class SceneBusComponent : ISceneComponent, IUpdatableComponent
    {
        private readonly IBus _bus;

        public SceneBusComponent(IBus bus)
        {
            _bus = bus;
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            //
        }

        [SequenceGroup<UpdateComponentSequenceGroup>(UpdateComponentSequenceGroup.PostUpdate)]
        public void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
