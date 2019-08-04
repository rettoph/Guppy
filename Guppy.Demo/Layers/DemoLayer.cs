using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy.Demo.Layers
{
    [IsLayer]
    public class DemoLayer : Layer
    {

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.logger.LogDebug($"Layer Updating!");
        }
    }
}
