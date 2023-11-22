using Guppy.GUI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    public interface IDebugComponent
    {
        void RenderDebugInfo(IGui gui, GameTime gameTime);
    }
}
