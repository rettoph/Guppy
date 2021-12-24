using Guppy.Example.Library.Layerables;
using Guppy.Extensions.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Examples.Client.Components.Layerables
{
    internal sealed class BallDrawComponent : DrawComponent<Ball>
    {
        protected override void Draw(GameTime gameTime)
        {
            this.primitiveBatch.TraceCircle(Color.DarkRed, this.Entity.Position, this.Entity.Radius);
            this.primitiveBatch.DrawCircle(Color.Red, this.Entity.Position, this.Entity.Radius);
        }
    }
}
