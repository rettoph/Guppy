using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Guppy.Game.ImGui
{
    internal sealed class DefaultImGuiObjectExplorer : ImGuiObjectExplorer
    {
        private readonly IObjectTextFilterService _filter;
        private readonly IImGui _imgui;
        private Dictionary<Type, (FieldInfo[], PropertyInfo[])> _typeInfo;

        private Dictionary<uint, TextFilterResult> _filterResults;
        private Vector4 _redForeground = Color.Red.ToVector4();
        private Vector4 _greenForeground = Color.LightGreen.ToVector4();
        private Vector4 _redBackground = Color.DarkRed.ToVector4();
        private Vector4 _greenBackground = Color.DarkGreen.ToVector4();

        public DefaultImGuiObjectExplorer(IObjectTextFilterService filter, IImGui imgui)
        {
            _filter = filter;
            _imgui = imgui;
            _typeInfo = new Dictionary<Type, (FieldInfo[], PropertyInfo[])>();
            _filterResults = new Dictionary<uint, TextFilterResult>();
        }

        public override bool AppliesTo(Type type)
        {
            return true;
        }

        public override TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            type = instance?.GetType() ?? type;

            if (type.IsPrimitive || type == typeof(string) || instance is null || currentDepth >= maxDepth || (type.IsValueType == false && tree.Add(instance) == false))
            {
                return this.DrawText(index, name, type, instance, filter);
            }

            var (fields, properties) = this.GetTypeInfo(type);
            if (properties.Length == 0 && fields.Length == 0 && instance is not IEnumerable)
            {
                return this.DrawText(index, name, type, instance, filter);
            }

            using (_imgui.ApplyID($"{nameof(DefaultImGuiObjectExplorer)}_{name}_{index}"))
            {
                uint id = _imgui.GetID(nameof(TextFilterResult));
                ref TextFilterResult result = ref this.GetFilterResult(id);
                string title = this.GetTitle(index, name, type, instance);
                Vector4? color = result switch
                {
                    TextFilterResult.NotMatched => _redBackground,
                    TextFilterResult.Matched => _greenBackground,
                    _ => null
                };

                result = this.BasicFilter(filter, name, type, instance);
                if (_imgui.CollapsingHeader($"{title}###header", color))
                {
                    _imgui.Indent();

                    foreach (PropertyInfo property in properties)
                    {
                        object? propertyValue = property.GetValue(instance);

                        result = result.Max(this.explorer.DrawObjectExplorer(null, property.Name, property.PropertyType, propertyValue, filter, maxDepth, currentDepth + 1, tree));
                    }

                    foreach (FieldInfo field in fields)
                    {
                        object? fieldValue = field.GetValue(instance);

                        result = result.Max(this.explorer.DrawObjectExplorer(null, field.Name, field.FieldType, fieldValue, filter, maxDepth, currentDepth + 1, tree));
                    }

                    if (instance is IEnumerable enumerable)
                    {
                        int itemIndex = 0;
                        foreach (var item in enumerable)
                        {
                            result = result.Max(this.explorer.DrawObjectExplorer(itemIndex++, null, item?.GetType() ?? typeof(object), item, filter, maxDepth, currentDepth + 1, tree));
                        }
                    }

                    _imgui.Unindent();
                }
                else
                {
                    var filterResult = _filter.Filter(instance, filter, maxDepth - currentDepth, 0, tree);
                    result = result.Max(filterResult);
                }

                return result;
            }
        }

        private TextFilterResult BasicFilter(string filter, string? name, Type type, object? instance)
        {
            if (filter.IsNullOrEmpty())
            {
                return TextFilterResult.None;
            }

            if (name is string nameString && nameString.Contains(filter))
            {
                return TextFilterResult.Matched;
            }

            if (type.AssemblyQualifiedName is string assembly && assembly.Contains(filter))
            {
                return TextFilterResult.Matched;
            }

            if (instance?.ToString() is string instanceString && instanceString.Contains(filter))
            {
                return TextFilterResult.Matched;
            }

            return TextFilterResult.NotMatched;
        }

        private TextFilterResult DrawText(int? index, string? name, Type type, object? instance, string filter)
        {
            TextFilterResult result = this.BasicFilter(filter, name, type, instance);
            string title = this.GetTitle(index, name, type, instance);

            switch (result)
            {
                case TextFilterResult.None:
                    _imgui.Text(title);
                    break;
                case TextFilterResult.Matched:
                    _imgui.TextColored(_greenForeground, title);
                    break;
                case TextFilterResult.NotMatched:
                    _imgui.TextColored(_redForeground, title);
                    break;
            }

            return result;
        }

        private string GetTitle(int? index, string? name, Type type, object? instance)
        {
            StringBuilder title = new StringBuilder();

            if (index is not null)
            {
                title.Append($"{index}: ");
            }

            title.Append($"{type.GetFormattedName()} ");

            if (name is not null)
            {
                title.Append($"{name} ");
            }

            title.Append("- ");

            if (instance is null)
            {
                title.Append("null");
            }
            else
            {
                title.Append(instance?.ToString());
            }

            return title.ToString();
        }

        private ref TextFilterResult GetFilterResult(uint id)
        {
            ref TextFilterResult result = ref CollectionsMarshal.GetValueRefOrAddDefault(_filterResults, id, out _);

            return ref result;
        }

        private (FieldInfo[], PropertyInfo[]) GetTypeInfo(Type type)
        {
            ref (FieldInfo[], PropertyInfo[]) info = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeInfo, type, out bool exists);

            if (exists)
            {
                return info;
            }

            info.Item1 = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => typeof(Delegate).IsAssignableFrom(x.FieldType) == false)
                .ToArray();

            info.Item2 = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(x => x.GetMethod!.GetParameters().Length == 0)
                .Where(x => typeof(Delegate).IsAssignableFrom(x.PropertyType) == false)
                .ToArray();

            return info;
        }
    }
}
