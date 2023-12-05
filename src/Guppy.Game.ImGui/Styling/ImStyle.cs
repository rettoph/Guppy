using Guppy.Common;
using Guppy.Game.ImGui.Helpers;
using Guppy.Game.ImGui.Styling.StyleValueResources;
using Guppy.Resources;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Styling
{
    public class ImStyle : IDisposable
    {
        internal List<ImStyleValue> _values = new List<ImStyleValue>();

        public void Set(ImGuiStyleVar var, float value)
        {
            _values.Add(new ImStyleVarFloatValue(var, value));
        }

        public void Set(ImGuiStyleVar var, Vector2 value)
        {
            _values.Add(new ImStyleVarVector2Value(var, value));
        }

        public void Set(ImGuiCol var, Color color)
        {
            _values.Add(new ImStyleColorValue(var, new Ref<Color>(color)));
        }

        public void Set(ImFontPtr font)
        {
            _values.Add(new ImStyleFontValue(new Ref<ImFontPtr>(font)));
        }

        public ImStyle Apply()
        {
            this.Push();

            return this;
        }

        public void Push()
        {
            foreach (ImStyleValue value in _values)
            {
                value.Push();
            }
        }

        public void Pop()
        {
            foreach (ImStyleValue value in _values)
            {
                value.Pop();
            }
        }

        public void Dispose()
        {
            this.Pop();
        }
    }
}
