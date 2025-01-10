using System.Drawing;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedCamera : ICamera, ICamera2D
    {
        public Matrix World => throw new NotImplementedException();

        public Matrix View => throw new NotImplementedException();

        public Matrix Projection => throw new NotImplementedException();

        public Matrix WorldViewProjection => throw new NotImplementedException();

        public BoundingFrustum Frustum => throw new NotImplementedException();

        public bool Center { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Zoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RectangleF ViewportBounds => throw new NotImplementedException();

        public bool Contains(in Matrix transformation)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            throw new NotImplementedException();
        }

        public Vector2 Project(Vector2 source)
        {
            throw new NotImplementedException();
        }

        public Vector3 Project(Vector3 source)
        {
            throw new NotImplementedException();
        }

        public Vector2 Unproject(Vector2 source)
        {
            throw new NotImplementedException();
        }

        public Vector3 Unproject(Vector3 source)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}