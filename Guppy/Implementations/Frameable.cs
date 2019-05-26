using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public abstract class Frameable : Initializable, IFrameable
    {
        public Frameable(ILogger logger) : base(logger)
        {
        }

        public Frameable(Guid id, ILogger logger) : base(id, logger)
        {
        }

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
