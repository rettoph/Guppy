using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IFrameable : IService
    {
        void TryDraw(GameTime gameTime);
        void TryUpdate(GameTime gameTime);
    }
}
