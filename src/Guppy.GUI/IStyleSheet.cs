using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public interface IStyleSheet
    {
        T Get<T>(Property property, Selector selector);
        void Set<T>(Property property, Selector selector, T value);
    }
}
