using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public delegate void Step(GameTime gameTime);

    public interface IFrameable
    {
        event Step OnPreDraw;
        event Step OnDraw;
        event Step OnPostDraw;

        event Step OnPreUpdate;
        event Step OnUpdate;
        event Step OnPostUpdate;

        void TryDraw(GameTime gameTime);
        void TryUpdate(GameTime gameTime);
    }
}
