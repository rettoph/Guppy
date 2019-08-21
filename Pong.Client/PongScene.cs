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
    [IsScene]
    public class PongScene : Scene
    {
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.logger.LogDebug("Updating! " + this.Id);
        }
    }
}
