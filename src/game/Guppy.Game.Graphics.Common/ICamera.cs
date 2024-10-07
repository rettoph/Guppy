using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.Common
{
    public interface ICamera : IDisposable
    {
        public Matrix World { get; }
        public Matrix View { get; }
        public Matrix Projection { get; }
        public Matrix WorldViewProjection { get; }
        public BoundingFrustum Frustum { get; }

        void Update(GameTime gameTime);

        bool Contains(in Matrix transformation);

        Vector2 Project(Vector2 source);
        Vector2 Unproject(Vector2 source);
        Vector3 Project(Vector3 source);
        Vector3 Unproject(Vector3 source);
    }
}
