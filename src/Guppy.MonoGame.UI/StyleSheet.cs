using Guppy.Common;
using Guppy.Common.Utilities;
using Guppy.MonoGame.UI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    internal sealed class StyleSheet : IStyleSheet
    {
        record StyleState(Style Style, ElementState State);
        record SelectorStyling(Selector Selector, StyleValue Styling);


        private Dictionary<StyleState, List<SelectorStyling>> _values;
        private Dictionary<Selector, IStyleProvider> _providers;

        public required string Name { get; init; }

        public StyleSheet()
        {
            _values = new Dictionary<StyleState, List<SelectorStyling>>();
            _providers = new Dictionary<Selector, IStyleProvider>();
        }

        public IStyleProvider GetProvider(Selector selector)
        {
            if(!_providers.TryGetValue(selector, out var provider))
            {
                provider = new StyleProvider(selector, this);
                _providers.Add(selector, provider);
            }

            return provider;
        }

        public StyleValue<T> Get<T>(Selector selector, Style style, ElementState state)
        {
            var value = this.GetStyleStateSelectorValues(style, state)
                .Where(x => x.Selector.Includes(selector))
                .MaxBy(x => x.Selector.Specificity)?
                .Styling;

            if(value is null)
            {
                return new StyleValue<T>(selector, style, state, false, default!);
            }

            if (value is not StyleValue<T> casted)
            {
                throw new NotImplementedException();
            }

            return casted;
        }

        public void Set<T>(Selector selector, Style style, ElementState state, T value)
        {
            ThrowIf.Type.IsNotAssignableFrom(style.Type, typeof(T));

            var styleValue = new StyleValue<T>(selector, style, state, true, value);

            var selectorValues = this.GetStyleStateSelectorValues(style, state);
            var selectorValue = selectorValues.FirstOrDefault(x => x.Selector == selector);

            if(selectorValue is not null)
            {
                selectorValues.Remove(selectorValue);
            }

            selectorValues.Add(new SelectorStyling(selector, styleValue));

            _providers.Clear();
        }

        private List<SelectorStyling> GetStyleStateSelectorValues(Style style, ElementState state)
        {
            StyleState ss = new StyleState(style, state);

            if(!_values.TryGetValue(ss, out var selectorValues))
            {
                selectorValues = new List<SelectorStyling> ();
                _values.Add(ss, selectorValues);
            }

            return selectorValues;
        }
    }
}
