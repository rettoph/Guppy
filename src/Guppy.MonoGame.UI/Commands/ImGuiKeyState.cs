using Guppy.Common;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Commands
{
    public class ImGuiKeyState : Message, ICommandData
    {
        public readonly ImGuiKey Key;
        public readonly ButtonState State;

        public ImGuiKeyState(ImGuiKey key, ButtonState state)
        {
            this.Key = key;
            this.State = state;
        }
    }
}
