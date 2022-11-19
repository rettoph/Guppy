using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public sealed class Text : Element
    {
        public string Value;

        public Text(string value)
        {
            this.Value = value;
        }

        protected override void InnerDraw(GameTime gameTime)
        {
            ImGui.Text(this.Value);
        }
    }
}
