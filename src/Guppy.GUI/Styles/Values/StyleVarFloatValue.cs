using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styles.Values
{
    public sealed class StyleVarFloatValue : StyleValue
    {
        public readonly ImGuiStyleVar Var;
        public readonly float Value;

        public StyleVarFloatValue(ImGuiStyleVar var, float value)
        {
            Var = var;
            Value = value;
        }

        public override void Push()
        {
            ImGui.PushStyleVar(Var, Value);
        }

        public override void Pop()
        {
            ImGui.PopStyleVar();
        }
    }
}
