﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common;
using Guppy.Game.Common.Systems;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Systems.Guppy
{
    public class SceneBusSystem(IBus bus) : ISceneSystem, IUpdateSystem
    {
        private readonly IBus _bus = bus;

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.PostUpdate)]
        public void Update(GameTime gameTime)
        {
            this._bus.Flush();
        }
    }
}