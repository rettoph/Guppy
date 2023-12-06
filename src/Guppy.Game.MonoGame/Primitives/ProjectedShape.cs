using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame.Primitives
{
    public class ProjectedShape : PrimitiveShape
    {
        private readonly Camera _camera;

        public ProjectedShape(Camera camera, IEnumerable<Vector2> vertices) : base(vertices)
        {
            _camera = camera;
        }

        public override void Transform(int index, in Color color, ref Matrix transformation, out VertexPositionColor output)
        {
            base.Transform(index, color, ref transformation, out output);

            output.Position = _camera.Project(output.Position);
        }
    }
}
