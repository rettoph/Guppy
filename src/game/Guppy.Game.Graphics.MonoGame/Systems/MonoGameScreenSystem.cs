﻿using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Systems.Scene
{
    public class MonoGameScreenSystem(IScreen screen) : ISceneSystem, IDrawSystem
    {
        private readonly IScreen _screen = screen;

        [SequenceGroup<DrawSequenceGroupEnum>(DrawSequenceGroupEnum.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            this._screen.Camera.Update(gameTime);
        }
    }
}