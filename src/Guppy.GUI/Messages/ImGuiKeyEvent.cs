﻿using Guppy.Common;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Messages
{
    internal sealed class ImGuiKeyEvent : Message<ImGuiKeyEvent>
    {
        public readonly ImGuiNET.ImGuiKey Key;
        public readonly bool Down;

        public ImGuiKeyEvent(ImGuiNET.ImGuiKey key, bool down)
        {
            Key = key;
            Down = down;
        }
    }
}