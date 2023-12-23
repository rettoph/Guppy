using Guppy.Common.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Guppy
{
    [Sequence<DrawSequence>(DrawSequence.PreDraw)]
    internal sealed class ScreenComponent : IGuppyComponent, IGuppyDrawable
    {
        private readonly IScreen _screen;

        public ScreenComponent(IScreen screen)
        {
            _screen = screen;
        }

        public void Initialize(IGuppy guppy)
        {
            //throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            _screen.Camera.Update(gameTime);
        }
    }
}
