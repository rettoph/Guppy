using Guppy.MonoGame.UI.Elements;
using Guppy.MonoGame.UI.Utilities.ImGuiPushValues;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Elements
{
    public static class ImGuiElementFontExtensions
    {
        public static TElement SetFont<TElement>(this TElement element, ImGuiFont? font)
            where TElement : Element
        {
            element.Font = font;

            return element;
        }
    }
}
