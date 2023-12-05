using Guppy.Game.ImGui.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui
{
    public partial struct ImGuiViewportPtr
    {
        public Vector2 Size => this.Value.Size;
    }
}
