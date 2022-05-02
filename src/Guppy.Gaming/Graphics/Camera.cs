using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics
{
    public abstract class Camera
    {
        private Boolean _dirty;
        private GraphicsDevice _graphics;

        protected bool dirty
        {
            get => _dirty;
            set => _dirty = value;
        }

        public Matrix World;
        public Matrix View;
        public Matrix Projection;
        public BoundingFrustum Frustum;

        public Camera(GraphicsDevice graphics)
        {
            _graphics = graphics;

            this.World = Matrix.Identity;
            this.View = Matrix.Identity;
            this.Projection = Matrix.Identity;

            this.TryClean(new GameTime(), true);

            this.Frustum = new BoundingFrustum(Matrix.Identity);
        }

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
            this.SetWorld(ref this.World);
            this.SetView(ref this.View);
            this.SetProjection(ref this.Projection);

            this.Frustum.Matrix = this.View * this.Projection;
        }

        protected abstract void SetWorld(ref Matrix world);
        protected abstract void SetView(ref Matrix view);
        protected abstract void SetProjection(ref Matrix projection);

        public Vector2 Project(Vector2 source)
            => _graphics.Viewport.Unproject(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public Vector2 Unproject(Vector2 source)
            => _graphics.Viewport.Project(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public Vector3 Project(Vector3 source)
            => _graphics.Viewport.Unproject(source, this.Projection, this.View, this.World);
        public Vector3 Unproject(Vector3 source)
            => _graphics.Viewport.Project(source, this.Projection, this.View, this.World);
    }
}
