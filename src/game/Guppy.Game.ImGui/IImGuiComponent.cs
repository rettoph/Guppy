using Guppy.Core.Common;
using Guppy.Game.ImGui.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui
{
    public interface IImGuiComponent : ISequenceable<GuiSequence>
    {
        void DrawImGui(GameTime gameTime);
    }
}
