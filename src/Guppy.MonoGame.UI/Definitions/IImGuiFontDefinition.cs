using Guppy.Resources.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Definitions
{
    public interface IImGuiFontDefinition
    {
        string Key { get; }
        string TrueTypeFontResourceName { get; }
        int SizePixels { get; }

        unsafe ImGuiFont BuildFont(IResourceProvider resources, ImGuiIOPtr io);
    }
}
