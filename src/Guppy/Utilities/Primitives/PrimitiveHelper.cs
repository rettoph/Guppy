using Guppy.Extensions.Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    /// <summary>
    /// Simple helper method useful for updating 
    /// or creating Primitives instances.
    /// </summary>
    public class PrimitiveHelper
    {
        #region Private Fields
        private List<Vector3> _vertices;
        #endregion

        #region Constructor
        private PrimitiveHelper()
        {
            _vertices = new List<Vector3>();
        }
        #endregion

        #region Methods
        private Primitive Build()
            => new Primitive(_vertices);

        public PrimitiveHelper FillPath(params Vector2[] vertices)
        {
            Debug.Assert(vertices.Length >= 3);

            for (Int32 i = 1; i < vertices.Length - 1; i++)
                this.AddTriangle(ref vertices[0], ref vertices[i], ref vertices[i + 1]);

            return this;
        }

        public PrimitiveHelper DrawPath(Single size, Single borderSegments, Boolean close, params Vector2[] vertices)
        {
            Debug.Assert(vertices.Length >= 2);
            Debug.Assert(borderSegments >= 2);

            List<Vector2> segment = new List<Vector2>();
            Vector2 width = new Vector2(size, 0);

            for (Int32 i=0; i<vertices.Length - 1; i++)
            {
                // Calculate 90 deg out from the start of the input line.
                var angle = vertices[i].Angle(vertices[i + 1]) - MathHelper.Pi / 2;
                segment.Add(vertices[i]);
                segment.Add(vertices[i] + width.Rotate(angle));
                segment.Add(vertices[i + 1] + width.Rotate(angle));
                segment.Add(vertices[i + 1]);
                
                this.FillPath(segment.ToArray());
                segment.Clear();

                if (i < vertices.Length - 2 && borderSegments > 0)
                { // if there is another segment to be drawn build a pretty corner...
                    var nextAngle = vertices[i + 1].Angle(vertices[i + 2]) - MathHelper.Pi / 2;
                    var bArcRads = ((nextAngle - angle) / borderSegments);
                    

                    segment.Add(vertices[i + 1]);
                    for(Int32 j=0; j<borderSegments; j++)
                        segment.Add(vertices[i + 1] + width.Rotate(angle + (bArcRads*j)));

                    segment.Add(vertices[i + 1] + width.Rotate(nextAngle));
                    

                    this.FillPath(segment.ToArray());
                    segment.Clear();
                }
            }

            return this;
        }

        public PrimitiveHelper AddTriangle(ref Vector2 v1, ref Vector2 v2, ref Vector2 v3)
        {
            _vertices.Add(new Vector3(v1, 0));
            _vertices.Add(new Vector3(v2, 0));
            _vertices.Add(new Vector3(v3, 0));

            return this;
        }
        #endregion

        #region Static Methods
        public static Primitive Create(Action<PrimitiveHelper> builder)
        {
            var helper = new PrimitiveHelper();
            builder(helper);
            return helper.Build();
        }
        #endregion
    }
}
