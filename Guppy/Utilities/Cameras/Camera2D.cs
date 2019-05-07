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
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private Vector2 _position;
        public RectangleF ViewportBounds { get; private set; }

        protected Boolean dirtyViewport;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Single Zoom { get; private set; }

        public Camera2D(GraphicsDevice graphics, GameWindow window) : base(graphics)
        {
            _window = window;
            _graphics = graphics;

            this.dirtyViewport = true;

            this.Position = Vector2.Zero;
            this.Zoom = 1f;

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        public override void Update(GameTime gameTime)
        {
            if(this.dirtyViewport)
            {
                this.ViewportBounds = this.buildViewportBounds();
                this.dirtyViewport = false;
                this.dirtyMatrices = true;
            }

            base.Update(gameTime);
        }

        #region Matrice Build Methods
        protected override Matrix buildWorld()
        {
            return Matrix.Identity;
        }

        protected override Matrix buildProjection()
        {
            return Matrix.CreateOrthographicOffCenter(
                    this.Position.X - this.ViewportBounds.Width / 2,
                    this.Position.X + this.ViewportBounds.Width / 2,
                    this.Position.Y + this.ViewportBounds.Height / 2,
                    this.Position.Y - this.ViewportBounds.Height / 2,
                    0f,
                    1f)
                * Matrix.CreateScale(this.Zoom);
        }

        protected override Matrix buildView()
        {
            return Matrix.Identity;
        }

        protected virtual RectangleF buildViewportBounds()
        {
            return new RectangleF(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height);
        }
        #endregion

        #region Utility Methods
        public void MoveTo(Single x, Single y)
        {
            _position.X = x;
            _position.Y = y;

            this.dirtyMatrices = true;
        }
        public void MoveTo(Vector2 pos)
        {
            _position.X = pos.X;
            _position.Y = pos.Y;

            this.dirtyMatrices = true;
        }
        public void MoveTo(ref Vector2 pos)
        {
            _position = pos;

            this.dirtyMatrices = true;
        }

        public void MoveBy(Single x, Single y)
        {
            _position.X += x;
            _position.Y += y;

            this.dirtyMatrices = true;
        }
        public void MoveBy(Vector2 pos)
        {
            _position += pos;

            this.dirtyMatrices = true;
        }

        public void ZoomBy(Single multiplier)
        {
            this.Zoom *= multiplier;

            this.dirtyMatrices = true;
        }
        public void ZoomTo(Single value)
        {
            this.Zoom = value;

            this.dirtyMatrices = true;
        }
        #endregion

        #region Event Handlers
        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.dirtyViewport = true;
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }
    }
}
