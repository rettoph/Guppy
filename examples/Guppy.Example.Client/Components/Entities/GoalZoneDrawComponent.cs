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
    internal sealed class GoalZoneDrawComponent : DrawComponent<GoalZone>
    {
        protected override void Draw(GameTime gameTime)
        {
            this.primitiveBatch.TraceRectangleF(Color.Blue, this.Entity.Bounds);
            this.primitiveBatch.DrawRectangleF(Color.DarkBlue, this.Entity.Bounds);
        }
    }
}
