using System.Drawing;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

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

        public override Matrix World => this._world;

        public override Matrix View => this._view;

        public override Matrix Projection => this._projection;

        public override Matrix WorldViewProjection => this._worldViewProjection;

        public override BoundingFrustum Frustum => this._frustrum;

        public bool Center
        {
            get => this._center;
            set
            {
                this._center = value;
                this._dirtyProjection = true;
                this._dirty = true;
            }
        }
        public Vector2 Position
        {
            get => this._position;
            set
            {
                this._position = value;
                this._dirtyWorld = true;
                this._dirty = true;
            }
        }
        public float Zoom
        {
            get => this._zoom;
            set
            {
                this._zoom = value;
                this._dirtyView = true;
                this._dirty = true;
            }
        }
        public RectangleF ViewportBounds => this._viewportBounds;

        public MonoGameCamera2D(IGraphicsDevice graphics, IGameWindow window) : base(graphics)
        {
            this._window = window;
            this._frustrum = new BoundingFrustum(Matrix.Identity);
            this._viewportBounds = new RectangleF(
                x: this.graphics.Value.Viewport.Bounds.X,
                y: this.graphics.Value.Viewport.Bounds.Y,
                width: this.graphics.Value.Viewport.Bounds.Width,
                height: this.graphics.Value.Viewport.Bounds.Height);

            this.Center = true;
            this.Zoom = 1;
            this.Position = Vector2.Zero;

            this._window.Value.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        protected override void Dispose(bool disposing) => this._window.Value.ClientSizeChanged -= this.HandleClientBoundsChanged;

        public override void Update(GameTime gameTime)
        {
            if (this._dirty == false)
            {
                return;
            }

            if (this._dirtyWorld == true)
            {
                this._world = Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0);
                this._dirtyWorld = false;
            }

            if (this._dirtyView == true)
            {
                this._view = Matrix.CreateScale(this.Zoom, this.Zoom, 1);
                this._dirtyView = false;
            }

            if (this._dirtyProjection == true)
            {
                if (this.Center)
                {
                    this._projection = Matrix.CreateOrthographicOffCenter(
                            0 - (this.ViewportBounds.Width / 2),
                            0 + (this.ViewportBounds.Width / 2),
                            0 + (this.ViewportBounds.Height / 2),
                            0 - (this.ViewportBounds.Height / 2),
                            -1000f,
                            1000f);
                }
                else
                {
                    this._projection = Matrix.CreateOrthographicOffCenter(
                        0,
                        this.ViewportBounds.Width,
                        this.ViewportBounds.Height,
                        0,
                        -1000f,
                        1000f);
                }

                this._dirtyProjection = false;
            }

            this._worldViewProjection = this._world * this._view * this._projection;
            this._frustrum.Matrix = this._worldViewProjection;

            this._dirty = false;
        }

        private void HandleClientBoundsChanged(object? sender, EventArgs e)
        {
            this._viewportBounds = new RectangleF(
                x: this.graphics.Value.Viewport.Bounds.X,
                y: this.graphics.Value.Viewport.Bounds.Y,
                width: this.graphics.Value.Viewport.Bounds.Width,
                height: this.graphics.Value.Viewport.Bounds.Height);

            this._dirtyProjection = true;
            this._dirty = true;
        }
    }
}