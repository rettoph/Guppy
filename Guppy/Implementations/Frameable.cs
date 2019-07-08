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

        public void Draw(GameTime gameTime)
        {
            this.draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            this.update(gameTime);
        }

        protected virtual void draw(GameTime gameTime)
        {
            //
        }
        protected virtual void update(GameTime gameTime)
        {
            //
        }
    }
}
