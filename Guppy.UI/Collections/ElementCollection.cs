using Guppy.Collections;
using Guppy.Factories;
using Guppy.UI.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Collections
{
    public class ElementCollection<TElement> : ConfigurableCollection<TElement>
        where TElement : IElement
    {
        private IElement _container;

        public ElementCollection(IElement container, ConfigurableFactory<TElement> factory, IServiceProvider provider) : base(factory, provider)
        {
            _container = container;
        }

        public override bool Add(TElement item)
        {
            if(base.Add(item))
            {
                item.Container = _container;
                _container.Dirty = true;
                return true;
            }

            return false;
        }

        public override bool Remove(TElement item)
        {
            return base.Remove(item);
        }
    }

    public sealed class ElementCollection : ElementCollection<IElement>
    {
        public ElementCollection(IElement container, ConfigurableFactory<IElement> factory, IServiceProvider provider) : base(container, factory, provider)
        {
        }
    }
}
