using Guppy.Common;
using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    internal sealed partial class StyleSheet : IStyleSheet
    {
        private readonly IResourceProvider _resources;
        private readonly Dictionary<(Property, Selector), IStyle> _cache = new();

        public StyleSheet(IResourceProvider resources)
        {
            _resources = resources;
        }

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

        public IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, T value)
        {
            _values.Add(new StyleValue(property, selector, state, value));

            return this;
        }

        public IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, string resource)
        {
            T value = _resources.Get<T>(resource).Value;
            return this.Set<T>(property, selector, state, value);
        }
    }
}
