using Guppy.MonoGame.UI.Elements;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    internal class StyleProvider : IStyleProvider
    {
        record StyleState(Style Style, ElementState State);

        private readonly StyleSheet _source;
        private Dictionary<StyleState, StyleValue> _values = new();

        public Selector Selector { get; }

        public IStyleSheet Source => _source;

        public StyleProvider(Selector selector, StyleSheet source)
        {
            _source = source;

            this.Selector = selector;
        }

        public bool TryGet<T>(Style style, ElementState state, [MaybeNullWhen(false)] out T value)
        {
            var key = new StyleState(style, state);

            if(!_values.TryGetValue(key, out var styleValue))
            {
                styleValue = _source.Get<T?>(this.Selector, style, state);
                _values.Add(key, styleValue);
            }

            if(!styleValue.Defined)
            {
                value = default!;
                return false;
            }

            if(styleValue is not StyleValue<T> casted)
            {
                throw new NotImplementedException();
            }

            value = casted.Value;
            return true;
        }
    }
}
