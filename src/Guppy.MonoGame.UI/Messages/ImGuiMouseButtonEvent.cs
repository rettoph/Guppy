using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Messages
{
    public sealed class ImGuiMouseButtonEvent : Message
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
