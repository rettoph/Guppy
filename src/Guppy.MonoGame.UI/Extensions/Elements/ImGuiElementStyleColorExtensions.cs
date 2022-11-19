using Guppy.MonoGame.UI.Elements;
using Guppy.MonoGame.UI.Utilities.ImGuiPushValues;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Elements
{
    public static class ImGuiElementStyleColorExtensions
    {
        public static TElement AddStyleColor<TElement>(this TElement element, ImGuiCol imGuiColor, ref Num.Vector4 value)
            where TElement : Element
        {
            element.ImGuiStyleColors.Add(new ImGuiStyleColValue(imGuiColor, ref value));

            return element;
        }

        public static TElement AddStyleColor<TElement>(this TElement element, ImGuiCol imGuiColor, Num.Vector4 value)
            where TElement : Element
        {
            element.ImGuiStyleColors.Add(new ImGuiStyleColValue(imGuiColor, ref value));

            return element;
        }

        public static TElement AddStyleColor<TElement>(this TElement element, ImGuiCol imGuiColor, Color color)
            where TElement : Element
        {
            return element.AddStyleColor(imGuiColor, color.ToNumericsVector4());
        }

        public static TElement SetStyleColor<TElement>(this TElement element, ImGuiCol imGuiColor, ref Num.Vector4 value)
            where TElement : Element
        {
            if (element.ImGuiStyleColors.TrySet<Num.Vector4>(imGuiColor, ref value))
            {
                return element;
            }

            element.AddStyleColor(imGuiColor, ref value);

            return element;
        }

        public static TElement SetStyleColor<TElement>(this TElement element, ImGuiCol imGuiColor, Num.Vector4 value)
            where TElement : Element
        {
            if (element.ImGuiStyleColors.TrySet<Num.Vector4>(imGuiColor, ref value))
            {
                return element;
            }

            element.AddStyleColor(imGuiColor, ref value);

            return element;
        }

        public static TElement SetStyleColor<TElement>(this TElement element, ImGuiCol imGuiColor, Color color)
            where TElement : Element
        {
            return element.SetStyleColor(imGuiColor, color.ToNumericsVector4());
        }

        public static Num.Vector4? GetStyleVar<T>(this Element element, ImGuiCol imGuiColor)
            where T : struct
        {
            return element.ImGuiStyleColors.Get<Num.Vector4>(imGuiColor);
        }
    }
}
