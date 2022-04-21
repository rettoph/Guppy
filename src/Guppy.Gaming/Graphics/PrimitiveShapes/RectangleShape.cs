using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics.PrimitiveShapes
{
    public sealed class RectangleShape : PrimitivePolygon
    {
        private float _width;
        private float _height;
        private Vector2 _position;
        private float _rotation;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                this.dirty |= DirtyFlags.Vertices | DirtyFlags.Transformation;
            }
        }
        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                this.dirty |= DirtyFlags.Transformation;
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                this.dirty |= DirtyFlags.Vertices | DirtyFlags.Transformation;
            }
        }
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                this.dirty |= DirtyFlags.Vertices | DirtyFlags.Transformation;
            }
        }

        public RectangleShape(Vector2 position, float rotation, float width, float height, Color color) : base(4)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Width = width;
            this.Height = height;
            this.Color = color;
        }


        protected override void CleanTransformation(ref Matrix transformation)
        {
            transformation = Matrix.CreateTranslation(_position.X, _position.Y, 0) * Matrix.CreateRotationZ(_rotation);
        }

        protected override void CleanVertices(ref Vector3[] vertices)
        {
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(0 + _width, 0, 0);
            vertices[2] = new Vector3(0 + _width, 0 + _height, 0);
            vertices[3] = new Vector3(0, 0 + _height, 0);
        }
    }
}
