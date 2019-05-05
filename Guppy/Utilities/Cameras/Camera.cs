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
    public abstract class Camera
    {

        public Matrix World      { get; protected set; }
        public Matrix View       { get; protected set; }
        public Matrix Projection { get; protected set; }

        public Camera()
        {
            this.Projection = Matrix.Identity;
            this.View       = Matrix.Identity;
            this.World      = Matrix.Identity;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.UpdateMatrices();
        }

        protected abstract void UpdateMatrices();
    }
}
