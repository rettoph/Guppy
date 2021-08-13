using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.Microsoft.Xna.Framework;

namespace Guppy.Utilities.Cameras
{
    public abstract class Camera : Service
    {
        #region Private Fields
        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        private Boolean _dirty;
        private GraphicsDevice _graphics;
        #endregion

        #region Protected Attributes
        protected bool dirty
        {
            get => _dirty;
            set => _dirty = value;
        }
        #endregion

        #region Public Attributes
        public Matrix World { get { return _world; } }
        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }
        public BoundingFrustum Frustum { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            _graphics = provider.GetService<GraphicsDevice>();

            _world = Matrix.Identity;
            _view = Matrix.Identity;
            _projection = Matrix.Identity;

            this.TryClean(new GameTime(), true);

            this.Frustum = new BoundingFrustum(Matrix.Identity);
        }
        #endregion

        #region Clean Methods
        public void TryClean(GameTime gameTime, Boolean force = false)
        {
            if (this.dirty || false)
            {
                this.dirty = false;
                this.Clean(gameTime);
            }
        }

        protected virtual void Clean(GameTime gameTime)
        {
            this.SetWorld(ref _world);
            this.SetView(ref _view);
            this.SetProjection(ref _projection);

            this.Frustum.Matrix = this.View * this.Projection;
        }
        #endregion

        #region Matrix Methods
        protected abstract void SetWorld(ref Matrix world);
        protected abstract void SetView(ref Matrix view);
        protected abstract void SetProjection(ref Matrix projection);
        #endregion

        #region Utility Methods
        public Vector2 Project(Vector2 source)
            => _graphics.Viewport.Project(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public Vector2 Unproject(Vector2 source)
            => _graphics.Viewport.Unproject(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public Vector3 Project(Vector3 source)
            => _graphics.Viewport.Project(source, this.Projection, this.View, this.World);
        public Vector3 Unproject(Vector3 source)
            => _graphics.Viewport.Unproject(source, this.Projection, this.View, this.World);
        #endregion
    }
}
