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

        internal IStyler BuildStyler(ImGuiBatch batcher, IResourceProvider resources)
        {
            return new Styler(_values.Select(x => x.GetStylerValue(batcher, resources)).ToList());
        }
    }
}
