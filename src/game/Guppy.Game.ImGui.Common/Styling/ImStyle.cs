using Guppy.Core.Common;
using Guppy.Game.ImGui.Common.Styling.StyleValues;
using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Game.ImGui.Common.Styling
{
    public class ImStyle
    {
        private readonly Dictionary<string, ImStyleValue> _keyedValues = [];
        private readonly List<ImStyleValue> _globalValues = [];

        public void Set(string key, ImGuiStyleVar var, float value)
        {
            _keyedValues.Add(key, new ImStyleVarFloatValue(key, var, value));
        }

        public void Set(string key, ImGuiStyleVar var, Vector2 value)
        {
            _keyedValues.Add(key, new ImStyleVarVector2Value(key, var, value));
        }

        public void Set(string key, ImGuiCol var, Color color)
        {
            _keyedValues.Add(key, new ImStyleColorValue(key, var, new Ref<Color>(color)));
        }

        public void Set(string key, ImFontPtr font)
        {
            _keyedValues.Add(key, new ImStyleFontValue(key, new Ref<ImFontPtr>(font)));
        }

        public void Set(ImGuiStyleVar var, float value)
        {
            _globalValues.Add(new ImStyleVarFloatValue(null, var, value));
        }

        public void Set(ImGuiStyleVar var, Vector2 value)
        {
            _globalValues.Add(new ImStyleVarVector2Value(null, var, value));
        }

        public void Set(ImGuiCol var, Color color)
        {
            _globalValues.Add(new ImStyleColorValue(null, var, new Ref<Color>(color)));
        }

        public void Set(ImFontPtr font)
        {
            _globalValues.Add(new ImStyleFontValue(null, new Ref<ImFontPtr>(font)));
        }

        internal bool TryGetValue(string key, [MaybeNullWhen(false)] out ImStyleValue value)
        {
            return _keyedValues.TryGetValue(key, out value);
        }

        public void Push()
        {
            foreach (ImStyleValue value in _globalValues)
            {
                value.Push();
            }
        }

        public void Pop()
        {
            foreach (ImStyleValue value in _globalValues)
            {
                value.Pop();
            }
        }

        internal void SetValues(List<ImStyleValue> values)
        {
            foreach (var keyedValue in values.Where(x => x.Key is not null))
            {
                _keyedValues.Add(keyedValue.Key!, keyedValue);
            }

            _globalValues.AddRange(values.Where(x => x.Key is null));
        }
    }
}
