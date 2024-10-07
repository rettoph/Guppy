using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace Guppy.Game.MonoGame.Common.Utilities.Cameras
{
    public class MonoGameCamera2D : MonoGameCamera, ICamera2D
    {
        private readonly IGameWindow _window;

        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;
        private Matrix _worldViewProjection;
        private RectangleF _viewportBounds;
        private Vector2 _position;
        private float _zoom;
        private bool _center;
        private bool _dirty;
        private bool _dirtyWorld;
        private bool _dirtyView;
        private bool _dirtyProjection;
        private readonly BoundingFrustum _frustrum;

        public override Matrix World => _world;

        public override Matrix View => _view;

        public override Matrix Projection => _projection;

        public override Matrix WorldViewProjection => _worldViewProjection;

        public override BoundingFrustum Frustum => _frustrum;

        public bool Center
        {
            get => _center;
            set
            {
                _center = value;
                _dirtyProjection = true;
                _dirty = true;
            }
        }
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _dirtyWorld = true;
                _dirty = true;
            }
        }
        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                _dirtyView = true;
                _dirty = true;
            }
        }
        public RectangleF ViewportBounds => _viewportBounds;

        public MonoGameCamera2D(IGraphicsDevice graphics, IGameWindow window) : base(graphics)
        {
            _window = window;
            _frustrum = new BoundingFrustum(Matrix.Identity);
            _viewportBounds = new RectangleF(
                x: this.graphics.Value.Viewport.Bounds.X,
                y: this.graphics.Value.Viewport.Bounds.Y,
                width: this.graphics.Value.Viewport.Bounds.Width,
                height: this.graphics.Value.Viewport.Bounds.Height);

            this.Center = true;
            this.Zoom = 1;
            this.Position = Vector2.Zero;

            _window.Value.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        public override void Dispose()
        {
            _window.Value.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }

        public override void Update(GameTime gameTime)
        {
            if (_dirty == false)
            {
                return;
            }

            if (_dirtyWorld == true)
            {
                _world = Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0);
                _dirtyWorld = false;
            }

            if (_dirtyView == true)
            {
                _view = Matrix.CreateScale(this.Zoom, this.Zoom, 1);
                _dirtyView = false;
            }

            if (_dirtyProjection == true)
            {
                if (this.Center)
                {
                    _projection = Matrix.CreateOrthographicOffCenter(
                            0 - this.ViewportBounds.Width / 2,
                            0 + this.ViewportBounds.Width / 2,
                            0 + this.ViewportBounds.Height / 2,
                            0 - this.ViewportBounds.Height / 2,
                            -1000f,
                            1000f);
                }
                else
                {
                    _projection = Matrix.CreateOrthographicOffCenter(
                        0,
                        this.ViewportBounds.Width,
                        this.ViewportBounds.Height,
                        0,
                        -1000f,
                        1000f);
                }

                _dirtyProjection = false;
            }

            _worldViewProjection = _world * _view * _projection;
            _frustrum.Matrix = _worldViewProjection;

            _dirty = false;
        }

        private void HandleClientBoundsChanged(object? sender, EventArgs e)
        {
            _viewportBounds = new RectangleF(
                x: this.graphics.Value.Viewport.Bounds.X,
                y: this.graphics.Value.Viewport.Bounds.Y,
                width: this.graphics.Value.Viewport.Bounds.Width,
                height: this.graphics.Value.Viewport.Bounds.Height);

            _dirtyProjection = true;
            _dirty = true;
        }
    }
}
