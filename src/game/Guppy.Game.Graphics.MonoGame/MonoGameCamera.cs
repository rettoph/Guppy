using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Common.Utilities.Cameras
{
    public abstract class MonoGameCamera(IGraphicsDevice graphics) : ICamera
    {
        protected IGraphicsDevice graphics = graphics;

        public abstract Matrix World { get; }
        public abstract Matrix View { get; }
        public abstract Matrix Projection { get; }
        public abstract Matrix WorldViewProjection { get; }
        public abstract BoundingFrustum Frustum { get; }

        public abstract void Dispose();

        public abstract void Update(GameTime gameTime);

        public virtual bool Contains(in Matrix transformation)
            => this.Frustum.Contains(transformation.GetBoudingSphere(5f)) != ContainmentType.Disjoint;
        public virtual Vector2 Project(Vector2 source)
            => this.graphics.Value.Viewport.Project(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public virtual Vector2 Unproject(Vector2 source)
            => this.graphics.Value.Viewport.Unproject(source.ToVector3(), this.Projection, this.View, this.World).ToVector2();
        public virtual Vector3 Project(Vector3 source)
            => this.graphics.Value.Viewport.Project(source, this.Projection, this.View, this.World);
        public virtual Vector3 Unproject(Vector3 source)
            => this.graphics.Value.Viewport.Unproject(source, this.Projection, this.View, this.World);
    }
}
