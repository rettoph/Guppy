using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public class Window : Container<ImGuiWindowFlags>
    {
        public Window(string name) : base(name)
        {
        }

        protected override bool BeginDrawContainer()
        {
            return ImGui.Begin(this.Name, this.Flags);
        }

        protected override void EndDrawContainer()
        {
            ImGui.End();
        }
    }
}
