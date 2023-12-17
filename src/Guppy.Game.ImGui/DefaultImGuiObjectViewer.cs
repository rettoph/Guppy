using Guppy.Common.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Guppy.Game.ImGui
{
    internal sealed class DefaultImGuiObjectViewer : ImGuiObjectViewer
    {
        private Dictionary<Type, (FieldInfo[], PropertyInfo[])> _typeInfo;
        private Dictionary<uint, bool> _headerStates;

        public DefaultImGuiObjectViewer(IObjectTextFilterService filter) : base(filter)
        {
            _typeInfo = new Dictionary<Type, (FieldInfo[], PropertyInfo[])>();
            _headerStates = new Dictionary<uint, bool>();
        }

        public override bool AppliesTo(Type type)
        {
            return true;
        }

        protected override bool InternalRenderObjectViewer(int? index, string? name, Type type, object instance, IImGui imgui, string filter, int maxDepth, int currentDepth)
        {
            string title = this.GetTitle(index, name, type, instance);

            type = instance?.GetType() ?? type;
            if (type.IsPrimitive || type == typeof(string))
            {
                return this.RenderTitle(imgui, filter, title);
            }

            var (fields, properties) = this.GetTypeInfo(type);
            if (properties.Length == 0 && fields.Length == 0 && instance is not IEnumerable)
            {
                return this.RenderTitle(imgui, filter, title);
            }

            bool filtered = filter.IsNotNullOrEmpty() && this.filter.Filter(instance, filter, 1, 0);
            imgui.PushID($"header_{type.GetFormattedName()}_{name}_{index}");
            ref bool wasFiltered = ref this.GetFiltered(imgui.GetID("header"));

            if(wasFiltered)
            {
                imgui.PushStyleColor(ImGuiCol.Header, Color.Green.ToVector4());
            }

            if (imgui.CollapsingHeader($"{title}###header"))
            {
                if (wasFiltered)
                {
                    imgui.PopStyleColor();
                }

                imgui.Indent();

                foreach (PropertyInfo property in properties)
                {
                    object? propertyValue = property.GetValue(instance);

                    filtered |= imgui.ObjectViewer(null, property.Name, property.PropertyType, propertyValue, filter, maxDepth, currentDepth + 1);
                }

                foreach (FieldInfo field in fields)
                {
                    object? fieldValue = field.GetValue(instance);

                    filtered |= imgui.ObjectViewer(null, field.Name, field.FieldType, fieldValue, filter, maxDepth, currentDepth + 1);
                }

                if (instance is IEnumerable enumerable)
                {
                    int itemIndex = 0;
                    foreach (var item in enumerable)
                    {
                        filtered |= imgui.ObjectViewer(itemIndex++, null, item?.GetType() ?? typeof(object), item, filter, maxDepth, currentDepth + 1);
                    }
                }

                imgui.Unindent();
            }
            else if (wasFiltered)
            {
                imgui.PopStyleColor();
            }

            wasFiltered = filtered;

            imgui.PopID();

            return filtered;
        }



        private ref bool GetFiltered(uint id)
        {
            return ref CollectionsMarshal.GetValueRefOrAddDefault(_headerStates, id, out _);
        }

        private (FieldInfo[], PropertyInfo[]) GetTypeInfo(Type type)
        {
            ref (FieldInfo[], PropertyInfo[]) info = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeInfo, type, out bool exists);

            if (exists)
            {
                return info;
            }

            info.Item1 = type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToArray();
            info.Item2 = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(x => x.GetMethod!.GetParameters().Length == 0)
                .ToArray();

            return info;
        }
    }
}
