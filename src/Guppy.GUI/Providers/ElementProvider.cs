using Guppy.GUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    internal sealed class ElementProvider : IElementProvider
    {
        private IList<Element> _elements = new List<Element>();

        void IElementProvider.Add(Element element)
        {
            _elements.Add(element);
        }

        void IElementProvider.Remove(Element element)
        {
            _elements.Remove(element);
        }

        public IEnumerable<Element> Match(Selector query)
        {
            return _elements.Where(e => query.Match(e.Selector));
        }

        public IEnumerable<T> Match<T>(params string[] names) where T : Element
        {
            Selector query = Selector.Create<T>(names);
            return _elements.OfType<T>().Where(e => query.Match(e.Selector));
        }
    }
}
