using Guppy.Common.Utilities;
using Guppy.GUI.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    internal partial class StyleSheet : IStyleValueProvider
    {
        private class StyleValue
        {
            public readonly Property Property;
            public readonly Selector Selector;
            public readonly ElementState State;
            public readonly object? Value;

            public StyleValue(Property property, Selector selector, ElementState state, object? value)
            {
                this.Property = property;
                this.Selector = selector;
                this.State = state;
                this.Value = value;
            }
        }

        private List<StyleValue> _values = new();

        public bool Get<T>(Property<T> property, Selector selector, ElementState state, [MaybeNullWhen(false)] out T value)
        {
            StyleValue result = this.GetMatches<T>(property, selector, state)
                .OrderByDescending(x => x.Selector.Match(selector))
                .FirstOrDefault();

            if (result?.Value is null)
            {
                value = default!;
                return false;
            }

            value = (T)result.Value;

            return true;
        }

        private IEnumerable<StyleValue> GetMatches<T>(Property<T> property, Selector selector, ElementState state)
        {
            foreach (StyleValue styleValue in _values)
            {
                if (styleValue.Property != property)
                {
                    continue;
                }

                if ((styleValue.State | state) != state)
                {
                    continue;
                }

                if (styleValue.Selector.Match(selector) == 0)
                {
                    continue;
                }

                yield return styleValue;
            }
        }
    }
}
