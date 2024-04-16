using Guppy.Core.Common;
using Guppy.Game.ImGui.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common
{
    public interface IImGuiComponent : ISequenceable<GuiSequence>
    {
        void DrawImGui(GameTime gameTime);
    }
}
