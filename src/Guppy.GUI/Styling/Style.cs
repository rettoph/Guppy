using Guppy.GUI.Styling.StylerValues;
using Guppy.GUI.Styling.StyleValueResources;
using Guppy.Resources;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Guppy.GUI.Styling
{
    public class Style
    {
        internal StyleValue[] _values = Array.Empty<StyleValue>();

        internal IGuiStyle BuildGuiStyle(IGui imgui, IResourceProvider resources)
        {
            return new GuiStyle(_values.Select(x => x.GetGuiStyleValue(imgui, resources)).ToList());
        }
    }
}
