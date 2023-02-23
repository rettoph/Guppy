using Guppy.Common.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    internal sealed class StyleValueProvider<T> : IStyleValueProvider<T>
    {
        private Dictionary<ElementState, T?> _values = new();

        public Style Style { get; }

        public IStyleProvider Provider { get; }

        public StyleValueProvider(Style style, IStyleProvider provider)
        {
            this.Style = style;
            this.Provider = provider;
        }

        public void Set(ElementState state, T? value)
        {
            foreach(var flag in Enum.GetValues<ElementState>())
            {
                if(!state.HasFlag(flag))
                {
                    continue;
                }

                _values[flag] = value;
            }
        }

        public void Clear(ElementState state)
        {
            foreach (var flag in Enum.GetValues<ElementState>())
            {
                if (!state.HasFlag(flag))
                {
                    continue;
                }

                _values.Remove(flag);
            }
        }


        public bool TryGet(ElementState state, out T? value)
        {
            foreach (var flag in Enum.GetValues<ElementState>())
            {
                if (!state.HasFlag(flag))
                {
                    continue;
                }

                if(_values.TryGetValue(flag, out value))
                {
                    return true;
                }
            }

            value = default!;
            return false;
        }

        void IStyleValueProvider.Set(ElementState state, object? value)
        {
            this.Set(state, (T?)value);
        }

        bool IStyleValueProvider.TryGet(ElementState state, out object? value)
        {
            bool result = this.TryGet(state, out T? casted);
            value = casted;

            return result;
        }
    }
}
