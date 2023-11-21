using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StyleValueResources
{
    internal abstract class StyleValue
    {
        internal abstract IStylerValue GetStylerValue(ImGuiBatch batcher, IResourceProvider resources);
    }
}
