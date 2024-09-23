using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.MonoGame.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [Sequence<DrawSequence>(DrawSequence.PreDraw)]
    internal sealed class ScreenComponent : SceneComponent, IDrawableComponent
    {
        private readonly IScreen _screen;

        public ScreenComponent(IScreen screen)
        {
            _screen = screen;
        }

        [Sequence<DrawComponentSequence>(DrawComponentSequence.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            _screen.Camera.Update(gameTime);
        }
    }
}
