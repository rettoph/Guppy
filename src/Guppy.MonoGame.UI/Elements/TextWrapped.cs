using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public sealed class TextWrapped : Element
    {
        public string Value;

        public TextWrapped(string value)
        {
            this.Value = value;
        }

        protected override void InnerDraw(GameTime gameTime)
        {
            ImGui.TextWrapped(this.Value);
        }
    }
}
