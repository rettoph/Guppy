using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Guppy.Extensions;
using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.Utilities.Cameras
{
    public class Camera2D : Camera
    {
        #region Private Fields
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private Vector2 _position;
        private Single _zoomTarget;
        private Vector2 _positionTarget;
        private Single _minZoom = Single.Epsilon;
        private Single _maxZoom = Single.MaxValue;
        #endregion

        #region Public Properties
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
        public Single ZoomLerpStrength = 0.015625f;
        public Single MoveLerpStrength = 0.015625f;
        public Single Zoom { get; set; } = 1;

        public Single ZoomTarget { get => _zoomTarget; }
        public Vector2 PositionTarget { get => _positionTarget; }
        public Single MaxZoom
        {
            get => _maxZoom;
            set
            {
                _maxZoom = value;
                _zoomTarget = MathHelper.Clamp(_zoomTarget, this.MinZoom, this.MaxZoom);
                this.Zoom = MathHelper.Clamp(this.Zoom, this.MinZoom, this.MaxZoom);
            }
        }
        public Single MinZoom
        {
            get => _minZoom;
            set
            {
                _minZoom = value;
                _zoomTarget = MathHelper.Clamp(_zoomTarget, this.MinZoom, this.MaxZoom);
                this.Zoom = MathHelper.Clamp(this.Zoom, this.MinZoom, this.MaxZoom);
            }
        }
        #endregion

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

        protected override void Uninitialize()
        {
            base.Uninitialize();

            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }
        #endregion

        #region Clean Methods
        protected override void Clean(GameTime gameTime)
        {
            this.ViewportBounds = this.buildViewportBounds();

            base.Clean(gameTime);

            if (MathHelper.Distance(this.Zoom, _zoomTarget) > 0.0001f)
            { // Lerp to the zoom target
                this.Zoom = MathHelper.Lerp(this.Zoom, _zoomTarget, Math.Min(1, this.ZoomLerpStrength * (Single)gameTime.ElapsedGameTime.TotalMilliseconds));
                this.dirty = true;
            }

            if (Vector2.Distance(this.Position, _positionTarget) > 0.001f)
            { // Lerp to the position target
                this.Position = Vector2.Lerp(this.Position, _positionTarget, Math.Min(1, this.MoveLerpStrength * (Single)gameTime.ElapsedGameTime.TotalMilliseconds));
                this.dirty = true;
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
            => new RectangleF(
                x: 0,
                y: 0,
                width: _graphics.Viewport.Bounds.Width,
                height: _graphics.Viewport.Bounds.Height);
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
                _zoomTarget = MathHelper.Clamp(_zoomTarget, this.MinZoom, this.MaxZoom);

                this.dirty = true;
            }
        }
        public void ZoomTo(Single value, Boolean lerp = true)
        {
            if (lerp && value != _zoomTarget)
            {
                _zoomTarget = MathHelper.Clamp(value, this.MinZoom, this.MaxZoom);

                this.dirty = true;
            }
            else if (!lerp)
            {
                _zoomTarget = MathHelper.Clamp(value, this.MinZoom, this.MaxZoom);
                this.Zoom = _zoomTarget;
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
