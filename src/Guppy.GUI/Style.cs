using Guppy.GUI.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    internal class Style<T> : IStyle<T>
    {
        private Dictionary<ElementState, T?> _values;
        private IStyleValueProvider _provider;

        public Property<T> Property { get; }

        public Selector Selector { get; }

        public T? Value => this[ElementState.None];

        Property IStyle.Property => this.Property;

        public virtual T? this[ElementState state] => this.GetValue(state);

        public Style(Property<T> property, Selector selector, IStyleValueProvider provider)
        {
            _values = new Dictionary<ElementState, T?>();
            _provider = provider;

            this.Property = property;
            this.Selector = selector;
        }

        public T? GetValue(ElementState state)
        {
            if (_values.TryGetValue(state, out T? value))
            {
                return value;
            }

            if (!_provider.Get(this.Property, this.Selector, state, out value))
            {
                _values[state] = default!;
                return default!;
            }

            _values[state] = value;
            return value;
        }

        public bool TryGetValue(ElementState state, [MaybeNullWhen(false)] out T value)
        {
            if(_values.TryGetValue(state, out value))
            {
                return value is not null;
            }

            if(!_provider.Get(this.Property, this.Selector, state, out value))
            {
                _values[state] = default!;
                return false;
            }

            _values[state] = value;
            return true;
        }
    }
}
