using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.GUI.Elements;

namespace Guppy.GUI
{
    public interface IStyleSheet
    {
        IStyle<T> Get<T>(Property<T> property, Element element);
        IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, T value, int priority = 0);
        IStyleSheet Set<T>(Property<T> property, Selector selector, ElementState state, string resource, int priority = 0);
    }
}
