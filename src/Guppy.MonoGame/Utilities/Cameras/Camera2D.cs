using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Guppy.MonoGame.Utilities.Cameras
{
    public class Camera2D : Camera, IDisposable
    {
        private readonly GameWindow _window;
        private Vector2 _position;
        private float _zoom;
        private float _zoomTarget;
        private Vector2 _positionTarget;
        private float _minZoom = float.Epsilon;
        private float _maxZoom = float.MaxValue;

        public RectangleF ViewportBounds { get; private set; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _positionTarget = value;
                this.dirty = true;
            }
        }

        /// <summary>
        /// When true, the position of the camera will be centered on the viewport.
        /// 
        /// Otherwise, the position represents the top left corner of the viewport.
        /// </summary>
        public bool Center { get; set; } = true;
        public float ZoomLerpStrength = 0.015625f;
        public float MoveLerpStrength = 0.015625f;
        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                _zoomTarget = value;
                this.dirty = true;
            }
        }

        public float ZoomTarget { get => _zoomTarget; }
        public Vector2 PositionTarget { get => _positionTarget; }
        public float MaxZoom
        {
            get => _maxZoom;
            set
            {
                _maxZoom = value;
                _zoomTarget = MathHelper.Clamp(_zoomTarget, this.MinZoom, this.MaxZoom);
                this.Zoom = MathHelper.Clamp(this.Zoom, this.MinZoom, this.MaxZoom);
            }
        }
        public float MinZoom
        {
            get => _minZoom;
            set
            {
                _minZoom = value;
                _zoomTarget = MathHelper.Clamp(_zoomTarget, this.MinZoom, this.MaxZoom);
                this.Zoom = MathHelper.Clamp(this.Zoom, this.MinZoom, this.MaxZoom);
            }
        }

        public Camera2D(GraphicsDevice graphics, GameWindow window) : base(graphics)
        {
            _window = window;

            this.Zoom = 1;
            this.Position = Vector2.Zero;

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        public void Dispose()
        {
            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }

        public override bool Clean(GameTime gameTime)
        {
            this.ViewportBounds = this.BuildViewportBounds();

            base.Clean(gameTime);

            var elapsedMilliseconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (MathHelper.Distance(this.Zoom, _zoomTarget) > 0.0001f)
            { // Lerp to the zoom target
                _zoom = MathHelper.Lerp(this.Zoom, _zoomTarget, Math.Min(1, this.ZoomLerpStrength * elapsedMilliseconds));
                return false;
            }

            if (Vector2.Distance(this.Position, _positionTarget) > 0.001f)
            { // Lerp to the position target
                _position = Vector2.Lerp(this.Position, _positionTarget, Math.Min(1, this.MoveLerpStrength * elapsedMilliseconds));
                return false;
            }

            return true;
        }

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

        protected virtual RectangleF BuildViewportBounds()
        {
            return new RectangleF(
                x: 0,
                y: 0,
                width: this.graphics.Viewport.Bounds.Width,
                height: this.graphics.Viewport.Bounds.Height);
        }

        public void MoveTo(float x, float y)
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

        public void MoveBy(float x, float y)
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

        public void ZoomBy(float multiplier)
        {
            if (multiplier != 1)
            {
                _zoomTarget *= multiplier;
                _zoomTarget = MathHelper.Clamp(_zoomTarget, this.MinZoom, this.MaxZoom);

                this.dirty = true;
            }
        }
        public void ZoomTo(float value)
        {
            if (value != _zoomTarget)
            {
                _zoomTarget = MathHelper.Clamp(value, this.MinZoom, this.MaxZoom);

                this.dirty = true;
            }
        }

        private void HandleClientBoundsChanged(object? sender, EventArgs e)
        {
            this.dirty = true;
        }
    }
}
