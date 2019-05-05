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

        public BasicEffect BasicEffect { get; protected set; }

        public Camera(BasicEffect effect)
        {
            // Create a new basic effect to handle the farseer coordinate transformation
            this.BasicEffect = effect;
            this.BasicEffect.VertexColorEnabled = true;
            this.BasicEffect.TextureEnabled = true;

            this.Projection = Matrix.Identity;
            this.View       = Matrix.Identity;
            this.World      = Matrix.Identity;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.UpdateMatrices();

            this.BasicEffect.Projection = this.Projection;
            this.BasicEffect.World      = this.World;
            this.BasicEffect.View       = this.View;
            this.BasicEffect.CurrentTechnique.Passes[0].Apply();
        }

        protected abstract void UpdateMatrices();
    }
}
