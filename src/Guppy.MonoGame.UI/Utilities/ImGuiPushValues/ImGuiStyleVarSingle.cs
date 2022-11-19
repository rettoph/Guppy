using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Utilities.ImGuiPushValues
{
    internal sealed class ImGuiStyleVarSingle : ImGuiPushValue<ImGuiStyleVar, float>
    {
        public ImGuiStyleVarSingle(ImGuiStyleVar what, ref float value) : base(what, ref value)
        {
        }

        public override void Push()
        {
            ImGui.PushStyleVar(this.What, this.Value);
        }
    }
}
