using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IFrameable : IInitializable
    {
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
