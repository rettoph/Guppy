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
        private IStyleProvider? _parent;
        private Dictionary<Style, IStyleValueProvider> _values = new();
        private Dictionary<Style, IStyleValueProvider> _cache = new();

        public IElement Element { get; }

        public StyleProvider(IElement element)
        {
            this.Element = element;
        }

        public bool TryGet<T>(Style style, ElementState state, [MaybeNullWhen(false)] out T value)
        {
            var cache = this.GetValueProvider<T>(_cache, style);
            if (cache.TryGet(state, out value))
            {
                return value is not null;
            }

            var values = this.GetValueProvider<T>(_values, style);
            if(values.TryGet(state, out value))
            {
                cache.Set(state, value);
                return value is not null;
            }

            if(style.Inherit && _parent is not null && _parent.TryGet(style, state, out value))
            {
                cache.Set(state, value);
                return true;
            }

            value = default!;
            cache.Set(state, null);
            return false;
        }

        public void Set<T>(Style style, T? value)
        {
            this.Set(style, ElementState.Defaut, value);
        }

        public void Set<T>(Style style, ElementState state, T? value)
        {
            this.GetValueProvider<T>(_values, style).Set(state, value);
            this.GetValueProvider<T>(_cache, style).Set(state, value);
        }

        public void Clear<T>(Style style)
        {
            this.Clear<T>(style, ElementState.Defaut);
        }

        public void Clear<T>(Style style, ElementState state)
        {
            this.GetValueProvider<T>(_values, style).Clear(state);
            this.GetValueProvider<T>(_cache, style).Clear(state);
        }

        public IEnumerable<IStyleValueProvider> All()
        {
            return _values.Values;
        }

        public void Inherit(IStyleProvider parent)
        {
            _parent = parent;

            this.Clean();
        }

        public void Clean()
        {
            _cache.Clear();
        }

        private IStyleValueProvider<T> GetValueProvider<T>(IDictionary<Style, IStyleValueProvider> providers, Style style)
        {
            if(!providers.TryGetValue(style, out var uncasted))
            {
                var provider = new StyleValueProvider<T>(style, this);
                providers.Add(style, provider);

                return provider;
            }

            if(uncasted is IStyleValueProvider<T> casted)
            {
                return casted;
            }

            throw new InvalidCastException(nameof(uncasted));
        }
    }
}
