using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Structs
{
    public class KeyData
    {
        public Keys Key { get; set; }
        public Boolean Pressed { get; set; }
        public DateTime PressedAt { get; set; }
        public DateTime LastPressedEventAt { get; set; }
        public Char Char { get; set; }
    }
}
