using Guppy.Implementations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Cameras
{
    /// <summary>
    /// Camera's are used primarily within scene layers
    /// for simple debug rendering. They may also be 
    /// utilized within custom cames for perspective management. 
    /// </summary>
    public abstract class Camera : TrackedDisposable
    {
        private GraphicsDevice _graphics;

        public Matrix World      { get; protected set; }
        public Matrix View       { get; protected set; }
        public Matrix Projection { get; protected set; }

        protected Boolean dirtyMatrices { get; set; }


        public Camera(GraphicsDevice graphics)
        {
            _graphics = graphics;
            this.dirtyMatrices = true;

            this.Projection = Matrix.Identity;
            this.View       = Matrix.Identity;
            this.World      = Matrix.Identity;
        }

        public virtual void Update(GameTime gameTime)
        {
            if(this.dirtyMatrices)
            {
                this.World = this.buildWorld();
                this.Projection = this.buildProjection();
                this.View = this.buildView();

                this.dirtyMatrices = false;
            }
        }

        protected abstract Matrix buildWorld();
        protected abstract Matrix buildProjection();
        protected abstract Matrix buildView();

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
