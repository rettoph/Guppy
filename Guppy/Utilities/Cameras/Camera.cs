using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Cameras
{
    public abstract class Camera : Frameable
    {
        #region Private Fields
        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        private GraphicsDevice _graphics;
        #endregion

        #region Protected Fields
        protected Boolean dirty { get; set; }
        #endregion

        #region Public Attributes
        public Matrix World { get { return _world; } }
        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }
        public BoundingFrustum Frustum { get; private set; }
        #endregion

        #region Constructor
        public Camera(GraphicsDevice graphics = null)
        {
            _graphics = graphics;

            _world = Matrix.Identity;
            _view = Matrix.Identity;
            _projection = Matrix.Identity;

            this.dirty = true;

            this.Frustum = new BoundingFrustum(Matrix.Identity);
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.TryCleanMatrices();
        }
        #endregion

        #region Matrix Methods
        protected abstract void SetWorld(ref Matrix world);
        protected abstract void SetView(ref Matrix view);
        protected abstract void SetProjection(ref Matrix projection);
        #endregion

        #region Utility Methods
        /// <summary>
        /// Immediately attempt to update the camera's internal matrices.
        /// </summary>
        /// <param name="force">Force the update even if the camera is not marked as dirty.</param>
        /// <returns></returns>
        public Boolean TryCleanMatrices(Boolean force = false)
        {
            if (this.dirty || force)
            {
                this.SetWorld(ref _world);
                this.SetView(ref _view);
                this.SetProjection(ref _projection);

                this.Frustum.Matrix = this.View * this.Projection;

                this.dirty = false;

                return true;
            }

            return false;
        }

        public Vector3 Project(Vector3 source)
        {
            return _graphics.Viewport.Project(source, this.Projection, this.View, this.World);
        }
        public Vector3 Unproject(Vector3 source)
        {
            return _graphics.Viewport.Unproject(source, this.Projection, this.View, this.World);
        }
        #endregion
    }
}
