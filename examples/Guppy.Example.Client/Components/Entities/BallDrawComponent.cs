using Guppy.Example.Library.Entities;
using Guppy.Extensions.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Client.Components.Entities
{
    internal sealed class BallDrawComponent : DrawComponent<Ball>
    {
        protected override void Draw(GameTime gameTime)
        {
            this.primitiveBatch.TraceCircle(Color.Green, this.Entity.Position, this.Entity.Radius);
            this.primitiveBatch.DrawCircle(Color.DarkGreen, this.Entity.Position, this.Entity.Radius);
        }
    }
}
