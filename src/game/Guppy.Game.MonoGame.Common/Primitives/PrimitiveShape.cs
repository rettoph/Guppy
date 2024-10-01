using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.Common.Primitives
{
    public abstract class PrimitiveShape<TVertexType>(int length) : IPrimitiveShape<TVertexType>
        where TVertexType : struct, IVertexType
    {
        public int Length { get; } = length;

        public abstract void Transform(int index, in Color color, ref Matrix transformation, out TVertexType output);
    }

    public class PrimitiveShape(IEnumerable<Vector3> vertices) : PrimitiveShape<VertexPositionColor>(vertices.Count())
    {
        public readonly Vector3[] Vertices = vertices.ToArray();

        public PrimitiveShape(IEnumerable<Vector2> vertices) : this(vertices.Select(x => x.ToVector3()))
        {
        }

        public override void Transform(int index, in Color color, ref Matrix transformation, out VertexPositionColor output)
        {
            output.Color = color;
            Vector3.Transform(ref Vertices[index], ref transformation, out output.Position);
        }
    }
}
