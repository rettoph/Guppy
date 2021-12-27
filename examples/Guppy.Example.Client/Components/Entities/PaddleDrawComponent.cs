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
    internal sealed class PaddleDrawComponent : DrawComponent<Paddle>
    {
        protected override void Draw(GameTime gameTime)
        {
            this.primitiveBatch.TraceRectangleF(Color.Red, this.Entity.Bounds);
            this.primitiveBatch.DrawRectangleF(Color.DarkRed, this.Entity.Bounds);
        }
    }
}
