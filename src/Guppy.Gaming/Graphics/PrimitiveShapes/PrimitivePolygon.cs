using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics.PrimitiveShapes
{
    public abstract class PrimitivePolygon : PrimitiveShape<VertexPositionColor>
    {
        [Flags]
        protected enum DirtyFlags
        {
            Clean = 0,
            Vertices = 1,
            Transformation = 2,
            Color = 4,
        }

        protected DirtyFlags dirty;

        private VertexPositionColor[] _lines;
        private VertexPositionColor[] _triangles;

        private Vector3[] _vertices;
        private Vector3[] _transformedVertices;

        private Matrix _transformation;

        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                this.dirty |= DirtyFlags.Color;
            }
        }

        protected PrimitivePolygon(int vertices)
        {
            Debug.Assert(vertices >= 3);

            _lines = new VertexPositionColor[vertices * 2];
            _triangles = new VertexPositionColor[(vertices - 2) * 3];

            _vertices = new Vector3[vertices];
            _transformedVertices = new Vector3[vertices];

            this.dirty = DirtyFlags.Clean;
        }

        protected internal override void Trace(IPrimitiveBatch<VertexPositionColor> primitiveBatch)
        {
            this.Clean();

            primitiveBatch.DrawLines(_lines);
        }

        protected internal override void Fill(IPrimitiveBatch<VertexPositionColor> primitiveBatch)
        {
            this.Clean();

            primitiveBatch.DrawTriangles(_triangles);
        }

        private void Clean()
        {
            if((this.dirty & DirtyFlags.Vertices) != 0)
            {
                this.CleanVertices(ref _vertices);
                this.dirty &= ~DirtyFlags.Vertices;
            }

            if ((this.dirty & DirtyFlags.Transformation) != 0)
            {
                this.CleanTransformation(ref _transformation);

                Vector3.Transform(_vertices, ref _transformation, _transformedVertices);

                for (int i = 0; i < _vertices.Length - 1; i++)
                {
                    _lines[i].Position = _transformedVertices[i];
                    _lines[i + 1].Position = _transformedVertices[i + 1];
                }

                _lines[_lines.Length - 2].Position = _transformedVertices[_transformedVertices.Length - 1];
                _lines[_lines.Length - 1].Position = _transformedVertices[0];

                for (int i = 1; i < _vertices.Length - 1; i++)
                {
                    _triangles[0].Position = _transformedVertices[0];
                    _triangles[i].Position = _transformedVertices[i];
                    _triangles[i + 1].Position = _transformedVertices[i + 1];
                }

                _triangles[_triangles.Length - 3].Position = _transformedVertices[0];
                _triangles[_triangles.Length - 2].Position = _transformedVertices[_transformedVertices.Length - 2];
                _triangles[_triangles.Length - 1].Position = _transformedVertices[_transformedVertices.Length - 1];

                this.dirty &= ~DirtyFlags.Transformation;
            }

            if ((this.dirty & DirtyFlags.Color) != 0)
            {
                for(int i=0; i<_lines.Length; i++)
                {
                    _lines[i].Color = _color;
                }

                for (int i = 0; i < _triangles.Length; i++)
                {
                    _triangles[i].Color = _color;
                }

                this.dirty &= ~DirtyFlags.Color;
            }
        }

        protected abstract void CleanTransformation(ref Matrix transformation);
        protected abstract void CleanVertices(ref Vector3[] vertices);
    }
}
