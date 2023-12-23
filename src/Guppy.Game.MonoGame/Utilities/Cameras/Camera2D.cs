using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Guppy.Game.MonoGame.Utilities.Cameras
{
    public class Camera2D : Camera, IDisposable
    {
        private float _deltaTimeSum;
        private readonly GameWindow _window;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _zoom;
        private Vector2 _targetPosition;
        private Vector2 _targetVelocity;
        private float _targetZoom;
        private float _minZoom = float.Epsilon;
        private float _maxZoom = float.MaxValue;
        private bool _dirtyViewportBounds;

        /// <summary>
        /// When true, the position of the camera will be centered on the viewport.
        /// 
        /// Otherwise, the position represents the top left corner of the viewport.
        /// </summary>
        public bool Center = true;

        public float ZoomDamp = 5f;
        public float MoveDamping = 1f;

        public RectangleF ViewportBounds { get; private set; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _targetPosition = value;
            }
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                _targetVelocity = value;
            }
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                _targetZoom = value;
            }
        }

        public Vector2 TargetPosition
        {
            get => _targetPosition;
            set
            {
                _targetPosition = value;
            }
        }

        public Vector2 TargetVelocity
        {
            get => _targetVelocity;
            set
            {
                _targetVelocity = value;
            }
        }

        public float TargetZoom
        {
            get => _targetZoom;
            set
            {
                _targetZoom = Math.Clamp(value, this.MinZoom, this.MaxZoom);
            }
        }

        public float MaxZoom
        {
            get => _maxZoom;
            set
            {
                _maxZoom = value;
                _targetZoom = MathHelper.Clamp(_targetZoom, this.MinZoom, this.MaxZoom);
                this.Zoom = MathHelper.Clamp(this.Zoom, this.MinZoom, this.MaxZoom);
            }
        }
        public float MinZoom
        {
            get => _minZoom;
            set
            {
                _minZoom = value;
                _targetZoom = MathHelper.Clamp(_targetZoom, this.MinZoom, this.MaxZoom);
                this.Zoom = MathHelper.Clamp(this.Zoom, this.MinZoom, this.MaxZoom);
            }
        }

        public Camera2D(GraphicsDevice graphics, GameWindow window) : base(graphics)
        {
            _window = window;
            _dirtyViewportBounds = true;

            this.Zoom = 1;
            this.Position = Vector2.Zero;

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        public void Dispose()
        {
            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }

        public override void Update(GameTime gameTime)
        {
            if (_dirtyViewportBounds)
            {
                this.ViewportBounds = this.BuildViewportBounds();
                _dirtyViewportBounds = false;
            }

            base.Update(gameTime);

            var delataTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (MathHelper.Distance(_zoom, _targetZoom) > 0.0001f)
            { // Lerp to the zoom target
                _zoom = MathHelper.Lerp(
                    value1: _zoom,
                    value2: _targetZoom,
                    amount: this.ZoomDamp * delataTime);
            }

            if (Vector2.Distance(_velocity, _targetVelocity) > 0.001f)
            { // Lerp to velocity target
                _velocity = Vector2.Lerp(
                    value1: _velocity,
                    value2: _targetVelocity,
                    amount: this.MoveDamping * delataTime);
            }

            _position += _velocity * delataTime;
            _targetPosition += _targetVelocity * delataTime;

            if (Vector2.Distance(_position, _targetPosition) > 0.001f)
            { // Lerp to the position target
                _position = Vector2.Lerp(
                    value1: _position,
                    value2: _targetPosition,
                    amount: this.MoveDamping * delataTime);
            }
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
                        1f);

                projection *= Matrix.CreateScale(this.Zoom);
            }
            else
            {
                projection = Matrix.CreateOrthographicOffCenter(
                    this.Position.X,
                    this.Position.X + this.ViewportBounds.Width,
                    this.Position.Y + this.ViewportBounds.Height,
                    this.Position.Y,
                    0f,
                    1f);

                projection *= Matrix.CreateScale(this.Zoom);
            }
        }

        protected override void SetView(ref Matrix view)
        {
            view = Matrix.Identity;
        }

        public override Vector3 Project(Vector3 source)
        {
            Matrix matrix = this.Frustum.Matrix;
            Vector3 vector = Vector3.Transform(source, matrix);
            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!WithinEpsilon(a, 1f))
            {
                vector.X = vector.X / a;
                vector.Y = vector.Y / a;
            }
            vector.X = (((vector.X + 1f) * 0.5f) * this.ViewportBounds.Width) + this.ViewportBounds.X;
            vector.Y = (((-vector.Y + 1f) * 0.5f) * this.ViewportBounds.Height) + this.ViewportBounds.Y;
            vector.Z = 0;
            return vector;
        }

        protected virtual RectangleF BuildViewportBounds()
        {
            return new RectangleF(
                x: this.graphics.Viewport.Bounds.X,
                y: this.graphics.Viewport.Bounds.Y,
                width: this.graphics.Viewport.Bounds.Width,
                height: this.graphics.Viewport.Bounds.Height);
        }

        private void HandleClientBoundsChanged(object? sender, EventArgs e)
        {
            _dirtyViewportBounds = true;
        }

        private static bool WithinEpsilon(float a, float b)
        {
            float num = a - b;
            return ((-1.401298E-45f <= num) && (num <= float.Epsilon));
        }
    }
}
