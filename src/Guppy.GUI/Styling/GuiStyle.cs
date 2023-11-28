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
    internal class GuiStyle : IGuiStyle
    {
        private List<IGuiStyleValue> _values;

        internal GuiStyle(List<IGuiStyleValue> values)
        {
            _values = values;
        }

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
            _values.Add(new GuiStyleColorValue(var, NumericsHelper.Convert(color)));
        }

        public GuiStyle Apply()
        {
            this.Push();

            return this;
        }

        public void Push()
        {
            foreach (IGuiStyleValue value in _values)
            {
                value.Push();
            }
        }

        public void Pop()
        {
            foreach (IGuiStyleValue value in _values)
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
