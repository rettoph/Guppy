using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Graphics.PrimitiveShapes
{
    public class PathShape : PrimitiveShape
    {
        public PathShape(Vector2 position, float rotation, XnaColor color, Vector3[] localVertices) : base(position, rotation, color, localVertices)
        {
        }

        protected override void CleanBufferSize(int vertices, out int lines, out int triangles)
        {
            lines = (vertices - 1) * 2;
            triangles = 0;
        }

        protected override void CleanBuffer(in Vector3[] worldVertices, in VertexPositionColor[] lines, in VertexPositionColor[] triangles, in Microsoft.Xna.Framework.Color color)
        {
            for(int i=0; i<worldVertices.Length - 1; i++)
            {
                lines[(i * 2) + 0].Position = worldVertices[i + 0];
                lines[(i * 2) + 0].Color = color;
                lines[(i * 2) + 1].Position = worldVertices[i + 1];
                lines[(i * 2) + 1].Color = color;
            }
        }

        protected override void CleanLocalVertices(in Vector3[] localVertices)
        {
            throw new NotImplementedException();
        }
    }
}
