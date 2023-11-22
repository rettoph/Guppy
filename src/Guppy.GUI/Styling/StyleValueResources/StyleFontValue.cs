using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(TrueTypeFont))]
    internal class StyleFontValue : StyleValue
    {
        public readonly Resource<TrueTypeFont> Resource;
        public readonly int Size;

        public StyleFontValue(Resource<TrueTypeFont> resource, int size)
        {
            Resource = resource;
            Size = size;
        }

        internal override IStylerValue GetStylerValue(IGui imgui, IResourceProvider resources)
        {
            return new StylerFontValue(imgui.GetFont(this.Resource, this.Size).ImFontPtr);
        }
    }
}
