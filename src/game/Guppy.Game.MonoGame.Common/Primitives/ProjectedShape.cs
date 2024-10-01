using Guppy.Game.MonoGame.Common.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.Common.Primitives
{
    public class ProjectedShape(Camera camera, IEnumerable<Vector2> vertices) : PrimitiveShape(vertices)
    {
        private readonly Camera _camera = camera;

        public override void Transform(int index, in Color color, ref Matrix transformation, out VertexPositionColor output)
        {
            base.Transform(index, color, ref transformation, out output);

            output.Position = _camera.Project(output.Position);
        }
    }
}
