using ImGuiNET;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Services
{
    internal sealed class ImguiObjectViewer : IImguiObjectViewer
    {
        public void Render(object value, string label = "", BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            this.Render(value.GetHashCode().ToString(), value, label, bindingFlags);
        }

        private void Render(string id, object value, string label = "", BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance, Type? type = null)
        { 
            type ??= value.GetType();
            var typeName = GetFriendlyTypeName(type);
            var prefix = $"{typeName} {label}";
            var valueString = string.Empty;

            if(value is null)
            {
                ImGui.Text($"{prefix}: null");
                return;
            }

            if (value is string stringValue)
            {
                ImGui.Text($"{prefix}: {stringValue}");
                return;
            }

            if (value is Type typeValue)
            {
                ImGui.Text($"{prefix}: {GetFriendlyTypeName(typeValue)}");
                return;
            }

            if (value.ToString() != value.GetType().ToString())
            {
                valueString = ": " + value.ToString();
            }

            if (ImGui.TreeNode(id, prefix + valueString))
            {
                this.RenderFields(value, type, id, bindingFlags);
                this.RenderProperties(value, type, id, bindingFlags);
                this.RenderEnumerable(id, value, bindingFlags);

                ImGui.TreePop();
            }
        }

        private void RenderFields(object instance, Type type, string id, BindingFlags bindingFlags)
        {
            this.RenderMemberInfo<FieldInfo>(
                source: type,
                id: id + "_fields",
                members: type.GetFields(BindingFlags.Public | BindingFlags.Instance),
                label: "Fields",
                instance: instance,
                valueGetter: (p, i) => p.GetValue(i),
                typeGetter: p => p.FieldType,
                bindingFlags: bindingFlags);
        }

        private void RenderProperties(object instance, Type type, string id, BindingFlags bindingFlags)
        {
            this.RenderMemberInfo<PropertyInfo>(
                source: type,
                id: id + "_properties",
                members: type.GetProperties(BindingFlags.Public | BindingFlags.Instance),
                label: "Properties",
                instance: instance,
                valueGetter: (p, i) => p.GetValue(i),
                typeGetter: p => p.PropertyType,
                bindingFlags: bindingFlags);
        }

        private void RenderEnumerable(string id, object value, BindingFlags bindingFlags)
        {
            if (value is IEnumerable enumerable)
            {
                if (ImGui.TreeNode(id + "_items", "Items"))
                {
                    int i = 0;
                    foreach (var item in enumerable)
                    {
                        this.Render(id + "_items_" + i, item, (i++).ToString(), bindingFlags);
                    }

                    ImGui.TreePop();
                }
            }
        }

        private void RenderMemberInfo<TMemberInfo>(Type source, string id, TMemberInfo[] members, object instance, string label, Func<TMemberInfo, object, object?> valueGetter, Func<TMemberInfo, Type> typeGetter, BindingFlags bindingFlags)
            where TMemberInfo : MemberInfo
        {
            if(members.Length == 0)
            {
                return;
            }

            if (ImGui.TreeNode($"{label} ({members.Length})"))
            {
                foreach (TMemberInfo member in members)
                {
                    var value = valueGetter(member, instance);
                    var type = typeGetter(member);
                    var memberLabel = member.DeclaringType! == source ? member.Name : GetFriendlyTypeName(member.DeclaringType!) + "." + member.Name;
                    if (value is null)
                    {
                        ImGui.Text($"{GetFriendlyTypeName(type)} {memberLabel}: null");
                        continue;
                    }

                    this.Render(id + $"_{memberLabel}", value, memberLabel, bindingFlags, type);
                }

                ImGui.TreePop();
            }
        }

        private static string GetFriendlyTypeName(Type type)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            if (!type.IsGenericType)
            {
                return type.Name ?? "";
            }

            var builder = new StringBuilder();
            var name = type.Name.Split('`')[0];
            builder.Append(name);
            builder.Append('<');
            var first = true;
            foreach (var arg in type.GetGenericArguments())
            {
                if (!first)
                {
                    builder.Append(',');
                }
                builder.Append(GetFriendlyTypeName(arg));
                first = false;
            }
            builder.Append('>');
            return builder.ToString();
        }
    }
}
