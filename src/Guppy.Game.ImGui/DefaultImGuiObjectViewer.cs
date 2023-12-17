using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Guppy.Game.ImGui
{
    internal sealed class DefaultImGuiObjectViewer : ImGuiObjectViewer
    {
        private Dictionary<uint, bool> _headerStates;

        public DefaultImGuiObjectViewer()
        {
            _headerStates = new Dictionary<uint, bool>();
        }

        public override bool AppliesTo(Type type)
        {
            return true;
        }

        protected override bool RenderObjectViewer(string title, Type type, object? instance, IImGui imgui, string? filter, int maxDepth, int currentDepth)
        {
            type = instance?.GetType() ?? type;
            if (type.IsPrimitive || type == typeof(string))
            {
                imgui.Text(title);
                return filter.IsNullOrEmpty() || title.Contains(filter!);
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).ToArray();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToArray();
            if(properties.Length == 0 && fields.Length == 0 && instance is not IEnumerable)
            {
                imgui.Text(title);
                return filter.IsNullOrEmpty() || title.Contains(filter!);
            }

            bool result = filter.IsNullOrEmpty() || title.Contains(filter!);

            ref bool open = ref this.GetHeaderOpen(imgui.GetID(title));
            if (filter.IsNullOrEmpty())
            {
                imgui.SetNextItemOpen(open);
            }

            if (imgui.CollapsingHeader(title))
            {
                imgui.Indent();

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetGetMethod()!.GetParameters().Length != 0)
                    {
                        continue;
                    }

                    object? propertyValue = property.GetValue(instance);

                    result |= imgui.ObjectViewer(null, property.Name, property.PropertyType, propertyValue, filter, maxDepth, currentDepth + 1);
                }

                foreach (FieldInfo field in fields)
                {
                    object? fieldValue = field.GetValue(instance);

                    result |= imgui.ObjectViewer(null, field.Name, field.FieldType, fieldValue, filter, maxDepth, currentDepth + 1);
                }

                if (instance is IEnumerable enumerable)
                {
                    int itemIndex = 0;
                    foreach (var item in enumerable)
                    {
                        result |= imgui.ObjectViewer(itemIndex++, null, item.GetType(), item, filter, maxDepth, currentDepth + 1);
                    }
                }

                imgui.Unindent();
            }

            if(filter.IsNotNullOrEmpty())
            {
                open = result;
            }

            return result;
        }

        private ref bool GetHeaderOpen(uint id)
        {
            return ref CollectionsMarshal.GetValueRefOrAddDefault(_headerStates, id, out _);
        }
    }
}
