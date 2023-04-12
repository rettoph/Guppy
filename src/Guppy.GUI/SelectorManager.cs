using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public sealed class SelectorManager
    {
        private IStyleSheet _styleSheet;
        private Selector _selector;

        internal SelectorManager(IStyleSheet styleSheet, Selector selector)
        {
            _styleSheet = styleSheet;
            _selector = selector;
        }

        public SelectorManager Set<T>(Property<T> property, T value)
        {
            _styleSheet.Set<T>(property, _selector, value);
            return this;
        }

        public SelectorManager Set<T>(Property<T> property, ElementState state, T value)
        {
            _styleSheet.Set<T>(property, _selector, state, value);
            return this;
        }

        public SelectorManager Set<T>(Property<T> property, string resource)
        {
            _styleSheet.Set<T>(property, _selector, resource);
            return this;
        }

        public SelectorManager Set<T>(Property<T> property, ElementState state, string resource)
        {
            _styleSheet.Set<T>(property, _selector, state, resource);
            return this;
        }
    }
}
