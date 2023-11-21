using Guppy.Common.Attributes;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [Sequence<DrawSequence>(DrawSequence.PreDraw)]
    internal sealed class ScreenComponent : IGuppyComponent, IDrawableComponent
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
