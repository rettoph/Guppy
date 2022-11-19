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
    public static class ImGuiElementStyleVarExtensions
    {
        public static TElement AddStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, ref float value)
            where TElement : Element
        {
            element.ImGuiStyleVars.Add(new ImGuiStyleVarSingle(imGuiStyleVar, ref value));

            return element;
        }

        public static TElement AddStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, float value)
            where TElement : Element
        {
            element.ImGuiStyleVars.Add(new ImGuiStyleVarSingle(imGuiStyleVar, ref value));

            return element;
        }

        public static TElement AddStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, ref Num.Vector2 value)
            where TElement : Element
        {
            element.ImGuiStyleVars.Add(new ImGuiStyleVarVector2(imGuiStyleVar, ref value));

            return element;
        }

        public static TElement AddStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, Num.Vector2 value)
            where TElement : Element
        {
            element.ImGuiStyleVars.Add(new ImGuiStyleVarVector2(imGuiStyleVar, ref value));

            return element;
        }

        public static TElement SetStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, ref float value)
            where TElement : Element
        {
            if (element.ImGuiStyleVars.TrySet<float>(imGuiStyleVar, ref value))
            {
                return element;
            }

            element.AddStyleVar(imGuiStyleVar, ref value);

            return element;
        }

        public static TElement SetStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, float value)
            where TElement : Element
        {
            if (element.ImGuiStyleVars.TrySet<float>(imGuiStyleVar, ref value))
            {
                return element;
            }

            element.AddStyleVar(imGuiStyleVar, ref value);

            return element;
        }

        public static TElement SetStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, ref Num.Vector2 value)
            where TElement : Element
        {
            if (element.ImGuiStyleVars.TrySet<Num.Vector2>(imGuiStyleVar, ref value))
            {
                return element;
            }

            element.AddStyleVar(imGuiStyleVar, ref value);

            return element;
        }

        public static TElement SetStyleVar<TElement>(this TElement element, ImGuiStyleVar imGuiStyleVar, Num.Vector2 value)
            where TElement : Element
        {
            if (element.ImGuiStyleVars.TrySet<Num.Vector2>(imGuiStyleVar, ref value))
            {
                return element;
            }

            element.AddStyleVar(imGuiStyleVar, ref value);

            return element;
        }

        public static T? GetStyleVar<T>(this Element element, ImGuiStyleVar imGuiStyleVar)
            where T : struct
        {
            return element.ImGuiStyleVars.Get<T>(imGuiStyleVar);
        }
    }
}
