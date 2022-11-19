using ImGuiNET;
using System.Numerics;

namespace Guppy.MonoGame.UI.Utilities.ImGuiPushValues
{
    internal sealed class ImGuiStyleColValue : ImGuiPushValue<ImGuiCol, Vector4>
    {
        public ImGuiStyleColValue(ImGuiCol what, ref Vector4 value) : base(what, ref value)
        {
        }

        public override void Push()
        {
            ImGui.PushStyleColor(this.What, this.Value);
        }
    }
}
