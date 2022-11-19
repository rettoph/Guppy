using Guppy.MonoGame.UI.Utilities;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public partial class Element
    {
        private ImGuiPushManager<ImGuiCol>? _imGuiStyleColors;
        public ImGuiPushManager<ImGuiCol> ImGuiStyleColors => _imGuiStyleColors ??= this.AddPushManager<ImGuiCol>(d => ImGui.PopStyleColor(d.Count));
    }
}
