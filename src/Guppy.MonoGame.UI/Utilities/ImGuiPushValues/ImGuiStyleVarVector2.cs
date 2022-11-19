using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Utilities.ImGuiPushValues
{
    internal sealed class ImGuiStyleVarVector2 : ImGuiPushValue<ImGuiStyleVar, Num.Vector2>
    {
        public ImGuiStyleVarVector2(ImGuiStyleVar what, ref Num.Vector2 value) : base(what, ref value)
        {
        }

        public override void Push()
        {
            ImGui.PushStyleVar(this.What, this.Value);
        }
    }
}
