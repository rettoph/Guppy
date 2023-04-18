using Guppy.GUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    public interface IElementProvider
    {
        internal void Add(Element element);
        internal void Remove(Element element);

        IEnumerable<Element> Match(Selector query);
        IEnumerable<T> Match<T>(params string[] names)
            where T : Element;
    }
}
