using Guppy.Common;
using Guppy.GUI.Enums;
using Guppy.MonoGame;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public interface IGuiComponent : ISequenceable<GuiSequence>
    {
        void DrawGui(GameTime gameTime);
    }
}
