using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class GraphicsDeviceExtensions
    {
        #region Scissor API
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
        #endregion

        #region Pixel API
        private static Dictionary<GraphicsDevice, Texture2D> _pixels = new Dictionary<GraphicsDevice, Texture2D>();

        public static Texture2D BuildPixel(this GraphicsDevice graphics, Color? color = null)
            => new Texture2D(graphics, 1, 1).Then(p =>
            {
                p.SetData<Color>(new Color[] { color ?? Color.White });
            });
        #endregion
    }
}
