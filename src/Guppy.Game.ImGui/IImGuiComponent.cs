using Guppy.Common;
using Guppy.Game.ImGui.Enums;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui
{
    public interface IImGuiComponent : ISequenceable<GuiSequence>
    {
        void DrawImGui(GameTime gameTime);
    }
}
