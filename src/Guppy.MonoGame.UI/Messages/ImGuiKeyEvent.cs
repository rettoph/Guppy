using Guppy.Common;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Messages
{
    public sealed class ImGuiKeyEvent : Message
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
