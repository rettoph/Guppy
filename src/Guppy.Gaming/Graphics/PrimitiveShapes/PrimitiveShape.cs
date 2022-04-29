using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Graphics.PrimitiveShapes
{
    public abstract class PrimitiveShape<TVertexType>
        where TVertexType : struct, IVertexType
    {
        protected internal abstract void Trace(IPrimitiveBatch<TVertexType> primitiveBatch);
        protected internal abstract void Fill(IPrimitiveBatch<TVertexType> primitiveBatch);
    }

    public abstract class PrimitiveShape : PrimitiveShape<VertexPositionColor>
    {
        [Flags]
        public enum DirtyFlags
        {
            Clean = 0,
            LocalVertices = 1,
            Transformation = 2,
            BufferSize = 4,
            Buffer = 8,
            Color = 16,
            All = LocalVertices | Transformation | BufferSize | Buffer | Color
        }

        private Vector3[] _localVertices = default!;
        private Vector2 _position = default!;
        private float _rotation = default!;
        private XnaColor _color = default!;

        private Vector3[] _worldVertices = default!;
        private VertexPositionColor[] _lines;
        private VertexPositionColor[] _triangles;

        private Matrix _transformation;

        protected DirtyFlags dirty;

        public Vector3[] LocalVertices
        {
            get => _localVertices;
            set
            {
                _localVertices = value;
                this.dirty |= DirtyFlags.BufferSize;
            }
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                if(_position == value)
                {
                    return;
                }

                _position = value;
                this.dirty |= DirtyFlags.Transformation;
            }
        }

        public float Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation == value)
                {
                    return;
                }

                _rotation = value;
                this.dirty |= DirtyFlags.Transformation;
            }
        }

        public XnaColor Color
        {
            get => _color;
            set
            {
                if (_color == value)
                {
                    return;
                }

                _color = value;
                this.dirty |= DirtyFlags.Color;
            }
        }

        public Matrix Transformation => _transformation;

        public PrimitiveShape(Vector2 position, float rotation, XnaColor color, Vector3[] localVertices)
        {
            _worldVertices = Array.Empty<Vector3>();
            _lines = Array.Empty<VertexPositionColor>();
            _triangles = Array.Empty<VertexPositionColor>();

            this.Position = position;
            this.Rotation = rotation;
            this.Color = color;
            this.LocalVertices = localVertices;
            this.dirty = DirtyFlags.All;
        }

        protected internal override void Fill(IPrimitiveBatch<VertexPositionColor> primitiveBatch)
        {
            this.Clean();

            primitiveBatch.DrawTriangles(_triangles);
        }

        protected internal override void Trace(IPrimitiveBatch<VertexPositionColor> primitiveBatch)
        {
            this.Clean();

            primitiveBatch.DrawLines(_lines);
        }

        private void Clean()
        {
            if(this.dirty == DirtyFlags.Clean)
            {
                return;
            }

            if((this.dirty & DirtyFlags.BufferSize) != 0)
            {
                this.CleanBufferSize(_localVertices.Length, out int lines, out int triangles);
                if(_lines.Length != lines)
                {
                    _lines = new VertexPositionColor[lines];
                }
                if (_triangles.Length != triangles)
                {
                    _triangles = new VertexPositionColor[triangles];
                }
            }

            if ((this.dirty & DirtyFlags.LocalVertices) != 0)
            {
                this.CleanLocalVertices(in _localVertices);
                this.dirty |= DirtyFlags.Buffer;
            }

            if ((this.dirty & DirtyFlags.Transformation) != 0)
            {
                this.CleanTransformation(out _transformation);
                this.dirty |= DirtyFlags.Buffer;
            }

            if ((this.dirty & DirtyFlags.Buffer) != 0)
            {
                if(_localVertices.Length != _worldVertices.Length)
                {
                    _worldVertices = new Vector3[_localVertices.Length];
                }

                Vector3.Transform(_localVertices, ref _transformation, _worldVertices);
                this.CleanBuffer(in _worldVertices, in _lines, in _triangles, in _color);
                this.dirty &= ~DirtyFlags.Color;
            }

            if ((this.dirty & DirtyFlags.Color) != 0)
            {
                this.CleanColor(in _lines, in _triangles, in _color);
            }

            this.dirty = DirtyFlags.Clean;
        }

        protected abstract void CleanBufferSize(int vertices, out int lines, out int triangles);
        protected abstract void CleanLocalVertices(in Vector3[] localVertices);

        protected virtual void CleanTransformation(out Matrix transformation)
        {
            transformation = Matrix.CreateTranslation(_position.X, _position.Y, 0) * Matrix.CreateRotationZ(_rotation);
        }

        protected abstract void CleanBuffer(in Vector3[] worldVertices, in VertexPositionColor[] lines, in VertexPositionColor[] triangles, in XnaColor color);

        protected virtual void CleanColor(in VertexPositionColor[] lines, in VertexPositionColor[] triangles, in XnaColor color)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Color = color;
            }

            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i].Color = color;
            }
        }
    }
}
