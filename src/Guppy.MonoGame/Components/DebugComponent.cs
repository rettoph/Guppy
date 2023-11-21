using Guppy.Attributes;
using Guppy.GUI;
using Guppy.GUI.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    internal sealed class DebugComponent : IGuppyComponent, IGuiComponent
    {
        private readonly ImFontPtr _font;

        public DebugComponent(IImFontProvider fonts)
        {
            _font = fonts.GetFontPtr(GUI.Resources.TrueTypeFonts.DiagnosticsFont, 25);
        }

        public void Initialize(IGuppy guppy)
        {
        }

        public void DrawGui()
        {
            //ImGui.ShowStyleEditor();

            ImGui.SetNextWindowSize(ImGui.GetMainViewport().Size);
            ImGui.PushFont(_font);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0f);
            ImGui.PushStyleColor(ImGuiCol.WindowBg, Color.Red);

            if (ImGui.Begin(nameof(DebugComponent), ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar))
            {

            }

            ImGui.PopStyleColor();
            ImGui.PopStyleVar();
            ImGui.PopFont();
            ImGui.End();
        }
    }
}
