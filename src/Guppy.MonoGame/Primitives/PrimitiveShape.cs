using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Primitives
{
    public abstract class PrimitiveShape<TVertexType> : IPrimitiveShape<TVertexType>
        where TVertexType : struct, IVertexType
    {
        public int Length { get; }

        protected PrimitiveShape(int length)
        {
            this.Length = length;
        }

        public abstract void Transform(int index, in Color color, ref Matrix transformation, out TVertexType output);
    }

    public class PrimitiveShape : PrimitiveShape<VertexPositionColor>
    {
        protected readonly Vector3[] _vertices;

        public PrimitiveShape(IEnumerable<Vector2> vertices) : this(vertices.Select(x => x.ToVector3()))
        {
        }
        public PrimitiveShape(IEnumerable<Vector3> vertices) : base(vertices.Count())
        {
            _vertices = vertices.ToArray();
        }

        public override void Transform(int index, in Color color, ref Matrix transformation, out VertexPositionColor output)
        {
            output.Color = color;
            Vector3.Transform(ref _vertices[index], ref transformation, out output.Position);
        }
    }
}
