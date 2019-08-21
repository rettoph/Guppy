using Guppy;
using Guppy.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    [IsLayer]
    public class PongLayer : Layer
    {
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.logger.LogDebug("Updating Layer " + this.Id);
        }
    }
}
