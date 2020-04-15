using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Guppy.Extensions;
using Guppy.Attributes;
using Guppy.Enums;
using Guppy.DependencyInjection;

namespace Guppy.Utilities.Cameras
{
    public class Camera2D : Camera
    {
        #region Private Fields
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private Vector2 _position;
        private Single _zoom;
        private Single _zoomTarget;
        private Vector2 _positionTarget;
        #endregion

        #region Public Attributes
        #endregion
        public RectangleF ViewportBounds { get; private set; }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// When true, the position of the camera will be centered on the viewport.
        /// 
        /// Otherwise, the position represents the top left corner of the viewport.
        /// </summary>
        public Boolean Center { get; set; } = true;
        public Single ZoomLerp = 0.015625f;
        public Single MoveLerp = 0.015625f;
        public Single Zoom { get; private set; } = 1;

        public Single ZoomTarget { get => _zoomTarget; }
        public Vector2 PositionTarget { get => _positionTarget; }

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _window = provider.GetService<GameWindow>();
            _graphics = provider.GetService<GraphicsDevice>();

            this.Position = Vector2.Zero;
            this.ZoomTo(1f);

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        protected override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }
        #endregion

        #region Clean Methods
        protected override void Clean(GameTime gameTime)
        {
            this.ViewportBounds = this.buildViewportBounds();

            base.Clean(gameTime);

            if (this.Zoom != _zoomTarget)
            { // Lerp to the zoom target
                this.Zoom = MathHelper.Lerp(this.Zoom, _zoomTarget, this.ZoomLerp * (Single)gameTime.ElapsedGameTime.TotalMilliseconds);
                this.EnqueueClean();
            }

            if (this.Position != _positionTarget)
            { // Lerp to the position target
                this.Position = Vector2.Lerp(this.Position, _positionTarget, this.MoveLerp * (Single)gameTime.ElapsedGameTime.TotalMilliseconds);
                this.EnqueueClean();
            }
        }
        #endregion

        #region Matrice Build Methods
        protected override void SetWorld(ref Matrix world)
        {
            world = Matrix.Identity;
        }

        protected override void SetProjection(ref Matrix projection)
        {
            if (this.Center)
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
            else
            {
                projection = Matrix.CreateOrthographicOffCenter(
                    this.Position.X,
                    this.Position.X + this.ViewportBounds.Width,
                    this.Position.Y + this.ViewportBounds.Height,
                    this.Position.Y,
                    0f,
                    1f)
                * Matrix.CreateScale(this.Zoom);
            }
        }

        protected override void SetView(ref Matrix view)
        {
            view = Matrix.Identity;
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
            this.dirty = true;
        }
        #endregion
    }
}
