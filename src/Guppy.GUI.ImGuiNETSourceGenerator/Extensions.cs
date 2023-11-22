using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy.GUI.ImGuiNETSourceGenerator
{
    internal static class Extensions
    {
        public static bool IsVoid(this Type type)
        {
            return type == typeof(void) || type.FullName == "System.Void";
        }

        public static string GetPrefix(this ParameterInfo parameter)
        {
            if(parameter.IsOut)
            {
                return "out ";
            }

            if (parameter.IsIn)
            {
                return "in ";
            }

            if (parameter.ParameterType.IsByRef)
            {
                return "ref ";
            }

            return "";
        }

        public static string Sanitize(this string value)
        {
            if (new[] { "ref", "in" }.Contains(value))
            {
                value = "@" + value;
            }


            return value.Replace("System.Void", "void");
        }

        public static string ToGuppyName(this string value)
        {
            if(value.Contains("ImGui"))
            {
                value = value.Replace("ImGui", "Gui");
            }

            if(value.StartsWith("Im"))
            {
                value = "Gui" + value.Substring(2);
            }


            return value;
        }
    }
}
