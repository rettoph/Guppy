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
        public readonly ImGuiCol Property;
        public readonly Resource<Color> Resource;

        public StyleColorValue(ImGuiCol col, Resource<Color> resource)
        {
            this.Property = col;
            this.Resource = resource;
        }

        internal override IStylerValue GetStylerValue(ImGuiBatch batcher, IResourceProvider resources)
        {
            return new StylerColorValue(Property, resources.Get(Resource).ToVector4());
        }
    }
}
