using Guppy.GUI.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public partial struct GuiViewportPtr
    {
        public Vector2 Size => this.Value.Size;
    }
}
