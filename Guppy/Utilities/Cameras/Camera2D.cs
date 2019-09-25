using Microsoft.Extensions.Logging;
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
        private Single _zoomTarget;
        private Vector2 _positionTarget;
        public RectangleF ViewportBounds { get; private set; }

        protected Boolean dirtyViewport;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Single ZoomLerp = 0.25f;
        public Single MoveLerp = 0.25f;
        public Single Zoom { get; private set; }

        public Single ZoomTarget { get => _zoomTarget; }
        public Vector2 PositionTarget { get => _positionTarget; }

        public Camera2D(GraphicsDevice graphics, GameWindow window) : base(graphics)
        {
            _window = window;
            _graphics = graphics;

            this.dirtyViewport = true;

            this.Position = Vector2.Zero;
            this.ZoomTo(1f);

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.dirtyViewport)
            {
                this.ViewportBounds = this.buildViewportBounds();
                this.dirtyViewport = false;
                this.dirty = true;
            }

            if (this.Zoom != _zoomTarget)
            {
                // Lerp to the zoom target
                this.Zoom = MathHelper.Lerp(this.Zoom, _zoomTarget, this.ZoomLerp);
                this.dirty = true;
            }

            if(this.Position != _positionTarget)
            {
                // Lerp to the position target
                this.Position = Vector2.Lerp(this.Position, _positionTarget, this.MoveLerp);
                this.dirty = true;
            }

            base.Update(gameTime);
        }

        #region Matrice Build Methods
        protected override void SetWorld(ref Matrix world)
        {
            // world = Matrix.Identity; 
        }

        protected override void SetProjection(ref Matrix projection)
        {
            projection = Matrix.CreateOrthographicOffCenter(
                    this.Position.X - this.ViewportBounds.Width / 2,
                    this.Position.X + this.ViewportBounds.Width / 2,
                    this.Position.Y + this.ViewportBounds.Height / 2,
                    this.Position.Y - this.ViewportBounds.Height / 2,
                    0f,
                    1f)
                * Matrix.CreateScale(this.Zoom);
        }

        protected override void SetView(ref Matrix view)
        {
            // view = Matrix.Identity;
        }

        protected virtual RectangleF buildViewportBounds()
        {
            return new RectangleF(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height);
        }
        #endregion

        #region Utility Methods
        public void MoveTo(Single x, Single y)
        {
            if (x != _position.X || y != _position.Y)
            {
                _positionTarget.X = x;
                _positionTarget.Y = y;

                this.dirty = true;
            }
        }
        public void MoveTo(Vector2 pos)
        {
            if (pos.X != _positionTarget.X || pos.Y != _positionTarget.Y)
            {
                _positionTarget.X = pos.X;
                _positionTarget.Y = pos.Y;

                this.dirty = true;
            }
        }
        public void MoveTo(ref Vector2 pos)
        {
            if (pos != _positionTarget)
            {
                _positionTarget = pos;

                this.dirty = true;
            }
        }

        public void MoveBy(Single x, Single y)
        {
            if (x != 0 || y != 0)
            {
                _positionTarget.X += x;
                _positionTarget.Y += y;

                this.dirty = true;
            }
        }
        public void MoveBy(Vector2 pos)
        {
            if (pos != Vector2.Zero)
            {
                _positionTarget += pos;

                this.dirty = true;
            }
        }

        public void ZoomBy(Single multiplier)
        {
            if (multiplier != 1)
            {
                _zoomTarget *= multiplier;

                this.dirty = true;
            }
        }
        public void ZoomTo(Single value)
        {
            if (value != _zoomTarget)
            {
                _zoomTarget = value;

                this.dirty = true;
            }
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
