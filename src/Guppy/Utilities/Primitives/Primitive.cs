using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    public sealed class Primitive
    {
        private Matrix _transformation;

        public Vector3[] Vertices { get; private set; }
        public Vector3[] Source { get; private set; }
        public Matrix Transformation
        {
            get => _transformation;
            set
            {
                if(value != _transformation)
                {
                    _transformation = value;
                    for(Int32 i=0; i<this.Source.Length; i++)
                        this.Vertices[i] = Vector3.Transform(this.Source[i], _transformation);
                }
            }
        }

        internal Primitive(IEnumerable<Vector3> vertices)
        {
            this.Source = vertices.ToArray();
            this.Vertices = new Vector3[this.Source.Length];
            this.Transformation = Matrix.Identity;
        }

        public Primitive SetTransformation(Matrix transformation)
        {
            this.Transformation = transformation;
            return this;
        }
    }
}
