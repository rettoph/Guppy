using Guppy.MonoGame.UI.Utilities;
using Guppy.MonoGame.UI.Utilities.ImGuiPushManagers;
using Guppy.MonoGame.UI.Utilities.ImGuiPushValues;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Elements
{
    public partial class Element
    {
        private ImGuiPushManager<ImGuiStyleVar>? _imGuiStyleVars;
        public ImGuiPushManager<ImGuiStyleVar> ImGuiStyleVars => _imGuiStyleVars ??= this.AddPushManager<ImGuiStyleVar>(d => ImGui.PopStyleVar(d.Count));
    }
}
