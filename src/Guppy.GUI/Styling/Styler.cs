using Guppy.GUI.Styling.StyleValueResources;
using Guppy.GUI.Styling.StylerValues;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.GUI.Helpers;

namespace Guppy.GUI.Styling
{
    public class Styler : IStyler
    {
        private List<IStylerValue> _values;

        internal Styler(List<IStylerValue> values)
        {
            _values = values;
        }

        public void Set(ImGuiStyleVar var, float value)
        {
            _values.Add(new StyleVarFloatValue(var, value));
        }

        public void Set(ImGuiStyleVar var, Vector2 value)
        {
            _values.Add(new StyleVarVector2Value(var, value));
        }

        public void Set(ImGuiCol var, Color color)
        {
            _values.Add(new StylerColorValue(var, NumericsHelper.Convert(color)));
        }

        public Styler Apply()
        {
            this.Push();

            return this;
        }

        public void Push()
        {
            foreach (IStylerValue value in _values)
            {
                value.Push();
            }
        }

        public void Pop()
        {
            foreach (IStylerValue value in _values)
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
