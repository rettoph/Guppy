using Guppy.Common;
using Guppy.Game.Input;
using Guppy.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Messages
{
    internal sealed class ImGuiMouseButtonEvent : Message<ImGuiMouseButtonEvent>, IInput
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
