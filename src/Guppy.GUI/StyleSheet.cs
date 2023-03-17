using Guppy.Common;
using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    internal sealed partial class StyleSheet : IStyleSheet
    {
        private Dictionary<(Property, Selector), IStyle> _cache = new();

        public IStyle<T> Get<T>(Property<T> property, Element element)
        {
            var key = (property, element.Selector);

            if(_cache.TryGetValue(key, out var value))
            {
                return Unsafe.As<IStyle<T>>(value);
            }

            var style = new Style<T>(property, element.Selector, this);
            _cache.Add(key, style);

            return style;
        }

        public IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, T value, int priority = 0)
        {
            _values.Add(priority, new StyleValue(property, selector, state, value));

            return this;
        }
    }
}
