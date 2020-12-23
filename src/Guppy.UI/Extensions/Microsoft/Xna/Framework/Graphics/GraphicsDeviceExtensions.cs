using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.Microsoft.Xna.Framework.Graphics
{
    public static class GraphicsDeviceExtensions
    {
        private static Stack<Rectangle> _scissors = new Stack<Rectangle>();

        public static void PushScissorRectangle(this GraphicsDevice graphics, Rectangle scissor)
        {
            _scissors.Push(graphics.ScissorRectangle);
            graphics.ScissorRectangle = graphics.ScissorRectangle.Intersection(scissor);
        }

        public static Rectangle PopScissorRectangle(this GraphicsDevice graphics)
        {
            graphics.ScissorRectangle = _scissors.Pop();
            return graphics.ScissorRectangle;
        }
    }
}
