using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Guppy.Utilities.Cameras
{
    /// <summary>
    /// Specific camera implementation used
    /// to generate 2d perspectives
    /// </summary>
    public class Camera2D : Camera
    {
        public Vector2 Position { get; private set; }
        public RectangleF Bounds { get; private set; }
        public Single Zoom { get; private set; }
        private GameWindow _window;

        public Camera2D(GameWindow window) : base()
        {
            _window = window;

            this.Position = new Vector2(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2);
            this.Bounds = new RectangleF(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            this.Zoom = 0.5f;

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.Bounds = new RectangleF(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            Console.WriteLine(this.Bounds.ToString());
        }

        protected override void UpdateMatrices()
        {
            this.World = Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0)
                * Matrix.CreateScale(this.Zoom)
                * Matrix.CreateTranslation(this.Bounds.Width / 2, this.Bounds.Height / 2, 0);

            this.Projection =
                Matrix.CreateOrthographicOffCenter(
                    this.Position.X - this.Bounds.Width / 2,
                    this.Position.X + this.Bounds.Width / 2,
                    this.Position.Y + this.Bounds.Height / 2,
                    this.Position.Y - this.Bounds.Height / 2,
                    0f,
                    1f);
        }
    }
}
