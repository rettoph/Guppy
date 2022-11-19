using Guppy.MonoGame.UI.Utilities.ImGuiPushValues;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Utilities.ImGuiPushManagers
{
    public sealed class ImGuiFontManager : ImGuiPushManager
    {
        public ImGuiFont Font;

        public ImGuiFontManager(ImGuiFont font)
        {
            this.Font = font;
        }

        public override void Push()
        {
            ImGui.PushFont(this.Font.Ptr);
        }

        public override void Pop()
        {
            ImGui.PopFont();
        }
    }
}
