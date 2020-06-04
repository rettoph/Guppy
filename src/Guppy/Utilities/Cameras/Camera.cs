using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.DependencyInjection;

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
        private UpdateBuffer _buffer;
        private Boolean _cleanEnqueued;
        #endregion

        #region Public Attributes
        public Matrix World { get { return _world; } }
        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }
        public BoundingFrustum Frustum { get; private set; }
        #endregion

        #region Protected Fields
        protected Boolean dirty
        {
            get => _dirty;
            set
            {
                if(_dirty != value)
                {
                    _dirty = value;

                    if (_dirty) // Auto Enqueue the clean method if needed.
                        this.EnqueueClean();
                }
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _graphics = provider.GetService<GraphicsDevice>();
            _buffer = provider.GetService<UpdateBuffer>();

            _world = Matrix.Identity;
            _view = Matrix.Identity;
            _projection = Matrix.Identity;

            this.dirty = true;

            this.Frustum = new BoundingFrustum(Matrix.Identity);
        }
        #endregion

        #region Clean Methods
        /// <summary>
        /// Enqueue the internal camera clean method, 
        /// if its not already.
        /// </summary>
        protected void EnqueueClean(Boolean next = false)
        {
            if(!_cleanEnqueued)
            {
                _buffer.Enqueue(this.TryClean, next);
                _cleanEnqueued = true;
            }
        }

        public void TryClean(GameTime gameTime)
        {
            if (this.dirty || _cleanEnqueued)
            {
                _cleanEnqueued = false;

                this.Clean(gameTime);
                this.dirty = false;
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
