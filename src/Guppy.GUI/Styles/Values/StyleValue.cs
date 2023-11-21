using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styles.Values
{
    public abstract class StyleValue
    {
        public abstract void Push();
        public abstract void Pop();
    }
}
