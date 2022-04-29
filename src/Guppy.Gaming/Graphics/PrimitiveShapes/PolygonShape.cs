using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Graphics.PrimitiveShapes
{
    public abstract class PolygonShape : PrimitiveShape
    {
        protected PolygonShape(Vector2 position, float rotation, XnaColor color, Vector3[] localVertices) : base(position, rotation, color, localVertices)
        {

        }

        protected override void CleanBufferSize(int vertices, out int lines, out int triangles)
        {
            lines = vertices * 2;
            triangles = (vertices - 2) * 3;
        }

        protected override void CleanBuffer(in Vector3[] worldVertices, in VertexPositionColor[] lines, in VertexPositionColor[] triangles, in XnaColor color)
        {
            for (int i = 0; i < worldVertices.Length - 1; i++)
            {
                lines[(i * 2) + 0].Position = worldVertices[i + 0];
                lines[(i * 2) + 0].Color = color;
                lines[(i * 2) + 1].Position = worldVertices[i + 1];
                lines[(i * 2) + 1].Color = color;
            }

            lines[lines.Length - 2].Position = worldVertices[worldVertices.Length - 1];
            lines[lines.Length - 2].Color = color;
            lines[lines.Length - 1].Position = worldVertices[0];
            lines[lines.Length - 1].Color = color;

            for (int i = 0; i < worldVertices.Length - 2; i++)
            {
                triangles[(i * 3) + 0].Position = worldVertices[0];
                triangles[(i * 3) + 0].Color = color;
                triangles[(i * 3) + 1].Position = worldVertices[i + 1];
                triangles[(i * 3) + 1].Color = color;
                triangles[(i * 3) + 2].Position = worldVertices[i + 2];
                triangles[(i * 3) + 2].Color = color;
            }

            triangles[triangles.Length - 3].Position = worldVertices[0];
            triangles[triangles.Length - 3].Color = color;
            triangles[triangles.Length - 2].Position = worldVertices[worldVertices.Length - 2];
            triangles[triangles.Length - 2].Color = color;
            triangles[triangles.Length - 1].Position = worldVertices[worldVertices.Length - 1];
            triangles[triangles.Length - 1].Color = color;
        }
    }
}
