using Guppy.Example.Library.Layerables;
using Guppy.Extensions.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Client.Components.Layerables
{
    internal sealed class PaddleDrawComponent : DrawComponent<Paddle>
    {
        protected override void Draw(GameTime gameTime)
        {
            var bounds = new System.Drawing.RectangleF(this.Entity.Position.X + 1, this.Entity.Position.Y + 1, Paddle.Width - 2, Paddle.Height - 2);

            this.primitiveBatch.TraceRectangleF(Color.DarkRed, bounds);
            this.primitiveBatch.DrawRectangleF(Color.Red, bounds);
        }
    }
}
