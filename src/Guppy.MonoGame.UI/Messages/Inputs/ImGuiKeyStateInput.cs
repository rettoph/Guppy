using Guppy.Common;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Messages.Inputs
{
    public class ImGuiKeyStateInput : Message
    {
        public readonly ImGuiKey Key;
        public readonly ButtonState State;

        public ImGuiKeyStateInput(ImGuiKey key, ButtonState state)
        {
            Key = key;
            State = state;
        }
    }
}
