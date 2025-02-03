using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Systems;
using Guppy.Game.Common.Enums;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    public class MonoGameScreenComponent(IScreen screen) : ISceneSystem, IDrawableSystem
    {
        private readonly IScreen _screen = screen;

        [SequenceGroup<DrawComponentSequenceGroupEnum>(DrawComponentSequenceGroupEnum.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            this._screen.Camera.Update(gameTime);
        }
    }
}