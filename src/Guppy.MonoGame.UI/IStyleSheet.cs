using Guppy.Common;
using Guppy.MonoGame.UI.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public interface IStyleSheet
    {
        string Name { get; }

        public IStyleProvider GetProvider(Selector selector);

        StyleValue<T> Get<T>(Selector selector, Style style, ElementState state);

        void Set<T>(Selector selector, Style style, ElementState state, T value);
    }
}
