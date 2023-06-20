using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.GUI.Elements;
using Guppy.Resources;

namespace Guppy.GUI
{
    public interface IStyleSheet
    {
        IStyle<T> Get<T>(Property<T> property, Element element);
        IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, T value);
        IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, Resource<T> resource)
            where T : notnull;
    }
}
