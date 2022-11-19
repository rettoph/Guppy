using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public class CollapsingHeader : Container<ImGuiTreeNodeFlags>
    {
        public CollapsingHeader(string name) : base(name)
        {
        }

        protected override bool BeginDrawContainer()
        {
            return ImGui.CollapsingHeader(this.Name, this.Flags);
        }

        protected override void EndDrawContainer()
        {
            //
        }
    }
}
