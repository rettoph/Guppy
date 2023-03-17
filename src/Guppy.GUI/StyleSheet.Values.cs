using Guppy.Common.Utilities;
using Guppy.GUI.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        private SortedList<int, StyleValue> _values = new(new DuplicateKeyComparer<int>());

        public bool Get<T>(Property<T> property, Selector selector, ElementState state, [MaybeNullWhen(false)] out T value)
        {
            StyleValue result = default!;

            foreach (KeyValuePair<int, StyleValue> styleValue in _values)
            {
                if (styleValue.Value.Property != property)
                {
                    continue;
                }

                if (!styleValue.Value.Selector.Match(selector))
                {
                    continue;
                }

                if ((styleValue.Value.State | state) != state)
                {
                    continue;
                }

                if (result is not null && styleValue.Value.State < result.State)
                {
                    continue;
                }

                result = styleValue.Value;
            }

            if (result?.Value is null)
            {
                value = default!;
                return false;
            }

            value = (T)result.Value;

            return true;
        }
    }
}
