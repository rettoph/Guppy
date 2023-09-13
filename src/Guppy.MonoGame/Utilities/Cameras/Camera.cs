using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Constants;

namespace Guppy.MonoGame.Utilities.Cameras
{
    public abstract class Camera
    {
        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        protected readonly GraphicsDevice graphics;

        public Matrix World { get { return _world; } }
        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }
        public BoundingFrustum Frustum { get; private set; }

        public Camera(GraphicsDevice graphics)
        {
            _world = Matrix.Identity;
            _view = Matrix.Identity;
            _projection = Matrix.Identity;

            this.graphics = graphics;

            this.Frustum = new BoundingFrustum(Matrix.Identity);

            this.Update(MiscConstants.EmptyGameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.SetWorld(ref _world);
            this.SetView(ref _view);
            this.SetProjection(ref _projection);

            this.Frustum.Matrix = this.View * this.Projection;
        }

        public virtual bool Contains(in Matrix transformation)
        {
            return this.Frustum.Contains(transformation.GetBoudingSphere(5f)) != ContainmentType.Disjoint;
        }

        #region Matrix Methods
        protected abstract void SetWorld(ref Matrix world);
        protected abstract void SetView(ref Matrix view);
        protected abstract void SetProjection(ref Matrix projection);
        #endregion

        #region Utility Methods
        public virtual Vector2 Project(Vector2 source)
            => this.graphics.Viewport.Project(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public virtual Vector2 Unproject(Vector2 source)
            => this.graphics.Viewport.Unproject(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public virtual Vector3 Project(Vector3 source)
            => this.graphics.Viewport.Project(source, this.Projection, this.View, this.World);
        public virtual Vector3 Unproject(Vector3 source)
            => this.graphics.Viewport.Unproject(source, this.Projection, this.View, this.World);
        #endregion
    }
}
