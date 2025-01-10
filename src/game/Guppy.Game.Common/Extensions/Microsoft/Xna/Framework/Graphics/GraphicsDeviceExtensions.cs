namespace Microsoft.Xna.Framework.Graphics
{
    public static class GraphicsDeviceExtensions
    {
        private static readonly Stack<Rectangle> _scissors = new();

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

        public static Texture2D BuildPixel(this GraphicsDevice graphics, Color? color = null)
            => new Texture2D(graphics, 1, 1).Then(p =>
            {
                p.SetData<Color>([color ?? Color.White]);
            });
    }
}