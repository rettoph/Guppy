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
    public sealed class RectangleShape : PolygonShape
    {
        private float _width;
        private float _height;

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                this.dirty |= DirtyFlags.LocalVertices;
            }
        }
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                this.dirty |= DirtyFlags.LocalVertices;
            }
        }

        public RectangleShape(float x, float y, float rotation, float width, float height, XnaColor color) : base(new Vector2(x, y), rotation, color, new Vector3[4])
        {
            this.Width = width;
            this.Height = height;
        }
        public RectangleShape(Vector2 position, float rotation, float width, float height, XnaColor color) : base(position, rotation, color, new Vector3[4])
        {
            this.Width = width;
            this.Height = height;
        }

        protected override void CleanLocalVertices(in Vector3[] localVertices)
        {
            localVertices[0] = new Vector3(0, 0, 0);
            localVertices[1] = new Vector3(0 + _width, 0, 0);
            localVertices[2] = new Vector3(0 + _width, 0 + _height, 0);
            localVertices[3] = new Vector3(0, 0 + _height, 0);
        }
    }
}
