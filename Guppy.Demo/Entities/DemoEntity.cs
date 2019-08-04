﻿using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Demo.Entities
{
    public class DemoEntity : Entity
    {
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.logger.LogDebug("Updating Entity!");
        }
    }
}