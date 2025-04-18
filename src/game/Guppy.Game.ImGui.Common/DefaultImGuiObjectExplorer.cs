﻿using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Guppy.Core.Common.Extensions.System;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common
{
    public sealed class DefaultImGuiObjectExplorer(IObjectTextFilterService filter, IImGui imgui) : ImGuiObjectExplorer
    {
        private readonly IObjectTextFilterService _filter = filter;
        private readonly IImGui _imgui = imgui;
        private readonly Dictionary<Type, (FieldInfo[], PropertyInfo[])> _typeInfo = [];

        private readonly Dictionary<uint, TextFilterResultEnum> _filterResults = [];
        private Vector4 _redForeground = Color.Red.ToVector4();
        private Vector4 _greenForeground = Color.LightGreen.ToVector4();
        private Vector4 _redBackground = Color.DarkRed.ToVector4();
        private Vector4 _greenBackground = Color.DarkGreen.ToVector4();

        public override bool AppliesTo(Type type)
        {
            return true;
        }

        public override TextFilterResultEnum DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree)
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

            using (this._imgui.ApplyID($"{nameof(DefaultImGuiObjectExplorer)}_{name}_{index}"))
            {
                uint id = this._imgui.GetID(nameof(TextFilterResultEnum));
                ref TextFilterResultEnum result = ref this.GetFilterResult(id);
                string title = GetTitle(index, name, type, instance);
                Vector4? color = result switch
                {
                    TextFilterResultEnum.NotMatched => this._redBackground,
                    TextFilterResultEnum.Matched => this._greenBackground,
                    _ => null
                };

                result = BasicFilter(filter, name, type, instance);
                if (this._imgui.CollapsingHeader($"{title}###header", color))
                {
                    this._imgui.Indent();

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
                        foreach (object? item in enumerable)
                        {
                            result = result.Max(this.explorer.DrawObjectExplorer(itemIndex++, null, item?.GetType() ?? typeof(object), item, filter, maxDepth, currentDepth + 1, tree));
                        }
                    }

                    this._imgui.Unindent();
                }
                else
                {
                    var filterResult = this._filter.Filter(instance, filter, maxDepth - currentDepth, 0, tree);
                    result = result.Max(filterResult);
                }

                return result;
            }
        }

        private static TextFilterResultEnum BasicFilter(string filter, string? name, Type type, object? instance)
        {
            if (filter.IsNullOrEmpty())
            {
                return TextFilterResultEnum.None;
            }

            if (name is string nameString && nameString.Contains(filter))
            {
                return TextFilterResultEnum.Matched;
            }

            if (type.AssemblyQualifiedName is string assembly && assembly.Contains(filter))
            {
                return TextFilterResultEnum.Matched;
            }

            if (instance?.ToString() is string instanceString && instanceString.Contains(filter))
            {
                return TextFilterResultEnum.Matched;
            }

            return TextFilterResultEnum.NotMatched;
        }

        private TextFilterResultEnum DrawText(int? index, string? name, Type type, object? instance, string filter)
        {
            TextFilterResultEnum result = BasicFilter(filter, name, type, instance);
            string title = GetTitle(index, name, type, instance);

            switch (result)
            {
                case TextFilterResultEnum.None:
                    this._imgui.Text(title);
                    break;
                case TextFilterResultEnum.Matched:
                    this._imgui.TextColored(this._greenForeground, title);
                    break;
                case TextFilterResultEnum.NotMatched:
                    this._imgui.TextColored(this._redForeground, title);
                    break;
            }

            return result;
        }

        private static string GetTitle(int? index, string? name, Type type, object? instance)
        {
            StringBuilder title = new();

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

        private ref TextFilterResultEnum GetFilterResult(uint id)
        {
            ref TextFilterResultEnum result = ref CollectionsMarshal.GetValueRefOrAddDefault(this._filterResults, id, out _);

            return ref result;
        }

        private (FieldInfo[], PropertyInfo[]) GetTypeInfo(Type type)
        {
            ref (FieldInfo[], PropertyInfo[]) info = ref CollectionsMarshal.GetValueRefOrAddDefault(this._typeInfo, type, out bool exists);

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