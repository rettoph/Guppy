using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Messages
{
    internal sealed class ImGuiMouseButtonEvent : Message<ImGuiMouseButtonEvent>
    {
        public readonly int Button;
        public readonly bool Down;

        public ImGuiMouseButtonEvent(int button, bool down)
        {
            this.Button = button;
            this.Down = down;
        }
    }
}
