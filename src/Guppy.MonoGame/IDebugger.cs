using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public interface IDebugger
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
