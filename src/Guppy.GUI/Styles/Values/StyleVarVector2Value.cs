using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styles.Values
{
    public sealed class StyleVarVector2Value : StyleValue
    {
        public readonly ImGuiStyleVar Var;
        public readonly Vector2 Value;

        public StyleVarVector2Value(ImGuiStyleVar var, Vector2 value)
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
