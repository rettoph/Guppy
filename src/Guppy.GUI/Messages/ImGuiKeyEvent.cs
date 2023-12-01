using Guppy.Common;
using Guppy.Input;
using Guppy.Messaging;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Messages
{
    internal sealed class ImGuiKeyEvent : Message<ImGuiKeyEvent>, IInput
    {
        public readonly ImGuiKey Key;
        public readonly bool Down;

        public ImGuiKeyEvent(ImGuiKey key, bool down)
        {
            Key = key;
            Down = down;
        }
    }
}
