using Guppy.Common;
using Guppy.GUI.Helpers;
using Guppy.GUI.Styling.StyleValueResources;
using Guppy.Resources;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Guppy.GUI.Styling
{
    public class Style : IDisposable
    {
        internal List<StyleValue> _values = new List<StyleValue>();

        public void Set(GuiStyleVar var, float value)
        {
            _values.Add(new StyleVarFloatValue(var, value));
        }

        public void Set(GuiStyleVar var, Vector2 value)
        {
            _values.Add(new StyleVarVector2Value(var, value));
        }

        public void Set(GuiCol var, Color color)
        {
            _values.Add(new StyleColorValue(var, new Ref<Color>(color)));
        }

        public void Set(GuiFontPtr font)
        {
            _values.Add(new StyleFontValue(new Ref<GuiFontPtr>(font)));
        }

        public Style Apply()
        {
            this.Push();

            return this;
        }

        public void Push()
        {
            foreach (StyleValue value in _values)
            {
                value.Push();
            }
        }

        public void Pop()
        {
            foreach (StyleValue value in _values)
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
