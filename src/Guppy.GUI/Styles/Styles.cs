using Guppy.GUI.Styles.Values;
using Guppy.Resources;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Guppy.GUI.Styles
{
    public class Styles : IDisposable
    {
        private List<StyleValue> _values = new List<StyleValue>();

        public void Set(ImGuiStyleVar var, float value)
        {
            _values.Add(new StyleVarFloatValue(var, value));
        }

        public void Set(ImGuiStyleVar var, Vector2 value)
        {
            _values.Add(new StyleVarVector2Value(var, value));
        }

        public void Set(ImGuiCol var, Color value)
        {
            _values.Add(new StyleColorValue(var, value));
        }

        public Styles Push()
        {
            foreach(StyleValue value in _values)
            {
                value.Push();
            }

            return this;
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
