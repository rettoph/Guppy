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

        public Camera2D(BasicEffect effect) : base(effect)
        {
        }

        protected override void UpdateMatrices()
        {
            this.World = Matrix.CreateTranslation(new Vector3(-this.Position.X, -this.Position.Y, 0)) *
                Matrix.CreateScale(this.Zoom) *
                Matrix.CreateTranslation(new Vector3(this.Bounds.Width * 0.5f, this.Bounds.Height * 0.5f, 0));

            this.Projection =
                Matrix.CreateOrthographicOffCenter(
                    this.Position.X / 2,
                    this.Position.X / 2,
                    this.Position.Y / 2,
                    this.Position.Y / 2,
                    0f,
                    1f) *
                Matrix.CreateScale(this.Zoom);
        }
    }
}
