using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public interface ISubscribableFrameable
    {
        public delegate void Step(GameTime gameTime);

        public event Step? OnDraw;
        public event Step? OnUpdate;
    }
}
