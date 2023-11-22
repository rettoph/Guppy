using Guppy.GUI.Helpers;
using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Color))]
    internal sealed class StyleColorValue : StyleValue
    {
        public readonly GuiCol Property;
        public readonly Resource<Color> Resource;

        public StyleColorValue(GuiCol col, Resource<Color> resource)
        {
            this.Property = col;
            this.Resource = resource;
        }

        internal override IStylerValue GetStylerValue(IGui imgui, IResourceProvider resources)
        {
            return new StylerColorValue(Property, NumericsHelper.Convert(resources.Get(Resource)));
        }
    }
}
