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
        public Frameable(IServiceProvider provider) : base(provider)
        {
        }

        public Frameable(Guid id, IServiceProvider provider) : base(id, provider)
        {
        }

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
