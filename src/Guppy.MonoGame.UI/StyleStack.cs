using Guppy.MonoGame.UI.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    internal sealed class StyleStack : IStyleStack
    {
        private Dictionary<Style, Stack<IStyleValueProvider>> _values = new();

        private Stack<Style> _styles = new();
        private Stack<int> _count = new();
        private Stack<IStyleProvider> _providers = new();
        private IStyleProvider? _latest;

        public bool TryGet<T>(Style style, ElementState state, [MaybeNullWhen(false)] out T value)
        {
            Stack<IStyleValueProvider> styleValues = this.GetValues(style);
            
            foreach(var values in styleValues)
            {
                if(values.Provider != _providers && style.Inherit == false)
                {
                    break;
                }

                if(values is not IStyleValueProvider<T> casted)
                {
                    throw new InvalidOperationException();
                }

                if(casted.TryGet(ElementState.Defaut, out value))
                {
                    return true;
                }
            }

            value = default!;
            return false;
        }

        public void Push(IStyleProvider provider)
        {
            int count = 0;

            foreach(var values in provider.All())
            {
                var styles = this.GetValues(values.Style);

                styles.Push(values);
                
                _styles.Push(values.Style);
                count++;
            }

            _count.Push(count);
            _providers.Push(provider);
            _latest = provider;
        }

        public void Pop()
        {
            int count = _count.Pop();

            for(int i=0; i<count; i++)
            {
                Style definition = _styles.Pop();
                _values[definition].Pop();
            }

            _providers.Pop();
            _latest = _providers.Peek();
        }

        private Stack<IStyleValueProvider> GetValues(Style definition)
        {
            if (!_values.TryGetValue(definition, out var styles))
            {
                return _values[definition] = new Stack<IStyleValueProvider>();
            }

            return styles;
        }
    }
}
