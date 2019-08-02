using Guppy.Attributes;
using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Demo.Drivers
{
    [IsDriver(typeof(Game))]
    public class DemoGuppyGameDriver : Driver<DemoGuppyGame>
    {
        public DemoGuppyGameDriver(DemoGuppyGame parent) : base(parent)
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
